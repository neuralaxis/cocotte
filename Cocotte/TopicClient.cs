using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Cocotte
{
    public class TopicClient : IDisposable, IPublisher, ISubscriber
    {
        readonly Encoding Encoding = Encoding.UTF8;

        readonly string _exchange;
        readonly string _consumer;
        readonly ManualResetEvent _wait;

        readonly IModel _model;
        readonly IConnection _connection;
        readonly ICollection<TopicSubscription> _subs = new List<TopicSubscription>();

        public TopicClient(Uri connectionString, string exchange, string consumer)
        {
            var factory = new ConnectionFactory();
            factory.SetUri(connectionString);

            _connection = factory.CreateConnection();
            _exchange = exchange;
            _consumer = consumer;
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(exchange, "topic");
            _wait = new ManualResetEvent(false);
        }

        public void Subscribe<T>(string routingKey, Action<T> handler)
        {
            Subscribe(routingKey, handler, (body) => {
                return JsonConvert.DeserializeObject<T>(Encoding.GetString(body));
            });
        }

        public void Subscribe(string routingKey, Action<string> handler)
        {
            Subscribe(routingKey, handler, (body) => {
                return Encoding.GetString(body);
            });
        }

        private void Subscribe<T>(string routingKey, Action<T> handler, Func<byte[], T> deserialize)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _subs.Add(new TopicSubscription(_connection, _exchange, _consumer, routingKey, (body) =>
             {
                 var msg = deserialize(body);
                 handler(msg);
             }));
        }

        public void Publish<T>(string routingKey, T msg)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            var data = Encoding.GetBytes(JsonConvert.SerializeObject(msg));
            _model.BasicPublish(_exchange, routingKey, body: data);
        }

        public void Wait()
        {
            _wait.WaitOne();
        }

        public void Dispose()
        {
            foreach (var sub in _subs) sub.Dispose();
            if (_model != null) _model.Dispose();
            if (_connection != null) _connection.Dispose();
        }
    }
}

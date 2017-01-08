using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocotte
{
    internal class TopicSubscription : IDisposable
    {
        readonly IModel _model;
        readonly EventingBasicConsumer _consumer;
        public TopicSubscription(IConnection conn, string exchange, string consumer, string routingKey, Action<byte[]> handler)
        {
            _model = conn.CreateModel();
            _model.ExchangeDeclare(exchange, "topic");

             var queue = _model.QueueDeclare($"{consumer}-{routingKey}").QueueName;
            _model.QueueBind(queue, exchange, routingKey);

            _consumer = new EventingBasicConsumer(_model);
            _model.BasicConsume(queue, true, _consumer);
            _consumer.Received += (model, ea) =>
            {
                handler(ea.Body);
            };
        }

        public void Dispose()
        {
            _model.Dispose();
        }
    }
}

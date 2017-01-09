using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cocotte
{
    /// <summary>
    /// Configuration options for Cocotte
    /// </summary>
    public class CocotteOptions 
    {
        private Uri _uri;
        private string _exchange;
        private string _consumer;
        internal IList<TopicHandlerSubscription> Handlers = new List<TopicHandlerSubscription>();

        public Uri Uri { get => _uri; set => _uri = value ?? throw new ArgumentNullException(nameof(value)); }
        public string Exchange { get => _exchange; set { if (String.IsNullOrEmpty(value)) { throw new ArgumentNullException(nameof(value)); } _exchange = value; } }
        public string Consumer { get => _consumer; set { if (String.IsNullOrEmpty(value)) { throw new ArgumentNullException(nameof(value)); } _consumer = value; } }

        public void AddHandler<THandler,TMessage>(string binding, THandler handler) where THandler: IHandler<TMessage>
        {
            Handlers.Add(new TopicHandlerSubscription { BindingKey = binding, Handler = handler });
        }
        internal CocotteOptions() { }

    }

    internal class TopicHandlerSubscription
    {
        public object Handler;
        public string BindingKey;
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddCocotte(this IServiceCollection svcs, Action<CocotteOptions> setup = null)
        {
            if (svcs == null) throw new ArgumentNullException(nameof(svcs));

            var opts = new CocotteOptions();
            setup?.Invoke(opts);

            if (opts.Uri == null) opts.Uri = new Uri("amqp://localhost:5672/");
            if (opts.Exchange == null) opts.Exchange = "events";
            if (opts.Consumer == null) opts.Consumer = Assembly.GetEntryAssembly().GetName().Name;

            var coco = new TopicClient(opts.Uri, opts.Exchange, opts.Consumer);
            svcs.AddSingleton<IPublisher>(coco);
            svcs.AddSingleton<ISubscriber>(coco);
            svcs.AddSingleton(coco);
        }
    }
}

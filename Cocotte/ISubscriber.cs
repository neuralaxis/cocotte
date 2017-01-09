using System;

namespace Cocotte
{
    public interface ISubscriber
    {
        void Subscribe<T>(string topic, Action<T> handler);
        void Subscribe(string topic, Action<string> obj);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cocotte
{
    public interface ISubscriber
    {
        void Subscribe<T>(string topic, Action<T> handler);
        void Subscribe(string topic, Action<string> obj);
    }
}

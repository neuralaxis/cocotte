using System;
using System.Collections.Generic;
using System.Text;

namespace Cocotte
{
    interface IPublisher
    {
        void Publish<T>(string topic, T msg);
    }
}

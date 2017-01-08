using System;
using System.Collections.Generic;
using System.Text;

namespace Cocotte
{
    public interface IHandler<T>
    {
        void Handle(T msg);
    }
}

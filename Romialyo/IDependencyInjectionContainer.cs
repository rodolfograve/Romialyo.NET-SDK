using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo
{
    public interface IDependencyInjectionContainer : IDisposable
    {
        T Get<T>();

        T Get<T>(params object[] parameters);

        object Get(Type t);

        object Get(Type t, params object[] parameters);

        object GetWithGenericArguments(Type genericType, params Type[] genericArguments);

        IEnumerable<Type> RegisteredTypes { get; }

    }
}

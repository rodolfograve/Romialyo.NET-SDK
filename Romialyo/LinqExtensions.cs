using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo
{
    public static class LinqExtensions
    {

        public static void ForEach<T>(this IEnumerable<T> target, Action<T> action)
        {
            foreach (var item in target)
            {
                action(item);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo
{
    public static class ConcurrencyExtensions
    {

        /// <summary>
        /// Executes <paramref name="lockedAction"/> inside a lock clause using 'this' as locker.
        /// </summary>
        /// <param name="target">Target of the invocation. This object is used as the locker.</param>
        /// <param name="lockedAction">Action to execute inside a lock clause.</param>
        public static void Lock(this object target, Action lockedAction)
        {
            lock (target)
            {
                lockedAction();
            }
        }

        /// <summary>
        /// Executes <paramref name="lockedFunction"/> inside a lock clause using 'this' as locker.
        /// </summary>
        /// <typeparam name="T">Type of the value returned by the Func <paramref name="lockedFunction"/>.</typeparam>
        /// <param name="target">Target of the invocation. This object is used as the locker.</param>
        /// <param name="lockedFunction">Func to execute inside a lock clause.</param>
        /// <returns></returns>
        public static T Lock<T>(this object target, Func<T> lockedFunction)
        {
            lock (target)
            {
                return lockedFunction();
            }
        }

    }
}

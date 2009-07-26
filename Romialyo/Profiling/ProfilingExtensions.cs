using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Profiling
{
    public static class ProfilingExtensions
    {

        public static void LogDuration(this ILogger logger, Action action)
        {
            DateTime start = DateTime.Now;
            try
            {
                action.Invoke();
            }
            finally
            {
                DateTime end = DateTime.Now;
                logger.Debug("PROFILE:'" + action.Method.Name + "' took " + end.Subtract(start).TotalMilliseconds.ToString() + " milliseconds.");
            }
        }

        public static T LogProfile<T>(this ILogger logger, Func<T> function)
        {
            DateTime start = DateTime.Now;
            try
            {
                T result = function.Invoke();
                return result;
            }
            finally
            {
                DateTime end = DateTime.Now;
                logger.Debug("PROFILE:'" + function.Method.Name + "' took " + end.Subtract(start).TotalMilliseconds.ToString() + " milliseconds.");
            }
        }

    }
}

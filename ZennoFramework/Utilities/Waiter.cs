using System;
using System.Threading;

namespace ZennoFramework.Utilities
{
    public static class Waiter
    {
        /// <summary>
        /// Ожидает, пока не будет выполнено выражение предиката.
        /// </summary>
        /// <param name="predicate">Условное выражение.</param>
        /// <param name="timeout">Длительность проверки выражения в миллисекундах.</param>
        /// <returns></returns>
        public static bool WaitFor(Func<bool> predicate, int timeout = 5000)
        {
            for (var i = 0; i < timeout / 100; i++)
            {
                if (predicate.Invoke())
                {
                    return true;
                }

                Thread.Sleep(100);
            }

            return false;
        }
    }
}
using System;
using System.Collections.Generic;

namespace ZennoFramework.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var obj in enumerable)
            {
                action(obj);
            }   
        }

        public static IEnumerable<TOut> ForEach<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> action)
        {
            var list = new List<TOut>();
            foreach (var obj in enumerable)
            {
                var newObj = action(obj);
                list.Add(newObj);
            }

            return list;
        }
    }
}
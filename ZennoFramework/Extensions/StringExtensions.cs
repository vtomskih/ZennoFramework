using System.Collections;
using System.Text;

namespace ZennoFramework.Extensions
{
    public static class StringExtensions
    {
        public static string Join(this IEnumerable values, string separator)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerator iter = values.GetEnumerator();
            while (iter.MoveNext())
            {
                object current = iter.Current;
                if (current != null)
                    sb.Append(current).Append(separator);
            }

            if (sb.Length == 0)
                return string.Empty;
            return sb.ToString(0, sb.Length - separator.Length);
        }

    }
}
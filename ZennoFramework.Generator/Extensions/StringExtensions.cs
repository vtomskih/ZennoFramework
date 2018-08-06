using System;

namespace ZennoFramework.Generator.Extensions
{
    public static class StringExtensions
    {
        public static string AddIndents(this string @this, string indent) =>
            indent + @this.AddIndentsWithoutFirst(indent);

        public static string AddIndentsWithoutFirst(this string @this, string indent) =>
            @this.Replace(Environment.NewLine, Environment.NewLine + indent);

        public static string NewLine(this string @this)
        {
            return @this + Environment.NewLine;
        }

        public static string NewLineSafe(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                return @this;
            }

            var code = @this;
            
            while (code.EndsWith(Environment.NewLine) || code.EndsWith(" "))
            {
                code = code.TrimEnd(Environment.NewLine.ToCharArray()).TrimEnd();
            }

            if (!code.EndsWith("{"))
            {
                @this += Environment.NewLine;
            }
            return @this;
        }
    }
}
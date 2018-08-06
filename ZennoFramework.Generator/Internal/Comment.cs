using System;

namespace ZennoFramework.Generator.Internal
{
    public static class Comment
    {
        internal static string GetComment(string comment)
        {
            comment = comment.Replace(Environment.NewLine, Environment.NewLine + "///");

            var result = string.Empty;
            result += $"/// <summary>{Environment.NewLine}";
            result += $"/// {comment} {Environment.NewLine}";
            result += $"/// </summary>";

            return result;
        }
    }
}
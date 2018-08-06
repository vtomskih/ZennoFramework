using System;
using ZennoFramework.Interception.Configuration;
using ZennoFramework.Utilities;

namespace ZennoFramework.Interception
{
    public static class InterceptionExtensions
    {
        public static void Add(this Interception @this, string interceptionName, Action action, InterceptorRulesBuilder rules = null)
        {
            Check.NotNull(action, nameof(action));
            @this.Add(interceptionName, new[] {action}, rules);
        }

        public static void Add(this Interception @this, string interceptionName, Action[] actions, InterceptorRulesBuilder rules = null)
        {
            Check.NotNull(actions, nameof(actions));
            @this.Add(new Interceptor(actions), interceptionName, rules);
        }
    }
}
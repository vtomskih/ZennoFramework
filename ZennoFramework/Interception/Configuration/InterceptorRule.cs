using System;

namespace ZennoFramework.Interception.Configuration
{
    public class InterceptorRule
    {
        public RuleType RuleType { get; set; }
        public bool WithNestedItems { get; set; }
        public Type Ancestor { get; set; }
        public Type Type { get; set; }
        public object Object { get; set; }
    }
}
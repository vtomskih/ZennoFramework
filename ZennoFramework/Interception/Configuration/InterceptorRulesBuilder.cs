using System.Collections.Generic;

namespace ZennoFramework.Interception.Configuration
{
    public class InterceptorRulesBuilder
    {
        private readonly List<InterceptorRule> _rules = new List<InterceptorRule>();

        public List<InterceptorRule> GetRules() => _rules;

        public RuleBuilder EnableFor(object obj)
        {
            var rule = new InterceptorRule {Object = obj, RuleType = RuleType.Enable};
            _rules.Add(rule);
            return new RuleBuilder(rule);
        }

        public RuleBuilder DisableFor(object obj)
        {
            var rule = new InterceptorRule {Object = obj, RuleType = RuleType.Disable};
            _rules.Add(rule);
            return new RuleBuilder(rule);
        }
    }
}
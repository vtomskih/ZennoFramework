namespace ZennoFramework.Interception.Configuration
{
    public class RuleBuilder
    {
        private readonly InterceptorRule _rule;

        public RuleBuilder(InterceptorRule rule)
        {
            _rule = rule;
        }

        public RuleBuilder WithNestedItems(bool value = true)
        {
            _rule.WithNestedItems = value;
            return this;
        }

        public RuleBuilder IfAncestor(object obj) 
        {
            _rule.Ancestor = obj.GetType();
            return this;
        }
    }
}           
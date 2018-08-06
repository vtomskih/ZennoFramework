using System;
using System.Collections.Generic;
using System.Linq;
using ZennoFramework.Interception.Abstractions;
using ZennoFramework.Interception.Configuration;
using ZennoFramework.Utilities;

namespace ZennoFramework.Interception
{
    public class Interception
    {
        private readonly Dictionary<string, InterceptorInfo> _interceptors = new Dictionary<string, InterceptorInfo>();
        private bool _isLocked;

        public Interception()
        {
            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }
        public IReadOnlyDictionary<string, InterceptorInfo> Interceptors => _interceptors;

        public void Add(IInterceptor interceptor, InterceptorRulesBuilder rulesBuilder = null)
        {
            Add(interceptor, GetName(interceptor), rulesBuilder);
        }

        public void Add(IInterceptor interceptor, string name, InterceptorRulesBuilder rulesBuilder = null)
        {
            Check.NotNull(interceptor, nameof(interceptor));
            Check.NotEmpty(name, nameof(name));

            if (Interceptors.ContainsKey(name))
            {
                throw new InterceptionException($"Интерсептор '{name}' уже зарегистрирован.");
            }

            var info = new InterceptorInfo {Name = name, Interceptor = interceptor, Rules = rulesBuilder?.GetRules()};
            _interceptors.Add(name, info);
        }

        private string GetName(IInterceptor interceptor)
        {
            if (interceptor is Interceptor)
            {
                var count = Interceptors.Values.Count(x => x.Interceptor is Interceptor);
                return $"Interceptor{count}";
            }

            return interceptor.GetType().Name;
        }

        public void Execute(object sender)
        {
            if (IsEnabled && !_isLocked)
            {
                _isLocked = true;

                foreach (var interceptorInfo in Interceptors.Values)
                {
                    if (CanExecute(interceptorInfo, sender))
                        interceptorInfo.Interceptor.Execute(sender);
                }

                _isLocked = false;
            }
        }

        private bool CanExecute(InterceptorInfo info, object sender)
        {
            if (sender is IInterceptable == false)
                return false;

            if (info.Rules == null || info.Rules.Count == 0)
                return true;

            var interceptable = (IInterceptable) sender;
            var types = interceptable.Info.StackTraceTypes;
            var rule = FindRule(types, info.Rules, sender);
            return rule == null || rule.RuleType == RuleType.Enable;
        }

        private InterceptorRule FindRule(List<Type> types, List<InterceptorRule> rules, object sender)
        {
            var rule = rules.FirstOrDefault(c => c.Object?.Equals(sender) == true);
            if (rule != null)
                return rule;

            foreach (var type in types)
            {
                var configs = rules.Where(c => c.Type == type || c.Object.GetType() == type).ToList();

                for (int i = configs.Count - 1; i >= 0; i--)
                {
                    rule = configs[i];

                    if (rule.Type == type || rule.Object.GetType() == type || rule.WithNestedItems)
                    {
                        if (rule.Ancestor == null || rule.Ancestor != null &&
                            types.FirstOrDefault(t => t == rule.Ancestor) != null)
                        {
                            return rule;
                        }
                    }
                }
            }

            return rules.LastOrDefault(c => c.Type == typeof(IInterceptable));
        }
    }
}
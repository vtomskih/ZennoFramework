using System;
using System.Collections.Generic;
using ZennoFramework.Interception.Abstractions;

namespace ZennoFramework.Interception
{
    public class InterceptorFactory : IInterceptorFactory
    {
        private readonly Dictionary<string, Interceptor> _interceptors =
            new Dictionary<string, Interceptor>(StringComparer.Ordinal);
        private readonly List<IInterceptorProvider> _providers = new List<IInterceptorProvider>();
        private readonly object _sync = new object();
        
        public IInterceptor CreateInterceptor(string categoryName)
        {
            lock (_sync)
            {
                if (!_interceptors.TryGetValue(categoryName, out var interceptor))
                {
                    interceptor = new Interceptor{Interceptors = CreateInterceptors(categoryName)};
                    _interceptors[categoryName] = interceptor;
                }

                return interceptor;
            }
        }

        public void AddProvider(IInterceptorProvider provider)
        {
            _providers.Add(provider);

            lock (_sync)
            {
                foreach (var interceptor in _interceptors)
                {
                    var interceptorInfo = interceptor.Value.Interceptors;
                    var categoryName = interceptor.Key;

                    Array.Resize(ref interceptorInfo, interceptorInfo.Length + 1);
                    var newLoggerIndex = interceptorInfo.Length - 1;

                    SetInterceptorInfo(ref interceptorInfo[newLoggerIndex], provider, categoryName);

                    interceptor.Value.Interceptors = interceptorInfo;
                }
            }
        }
        
        private void SetInterceptorInfo(ref InterceptorInfo interceptorInfo, IInterceptorProvider provider,
            string categoryName)
        {
            interceptorInfo.Interceptor = provider.CreateInterceptor(categoryName);
            interceptorInfo.ProviderType = provider.GetType();
        }

        private InterceptorInfo[] CreateInterceptors(string categoryName)
        {
            var interceptors = new InterceptorInfo[_providers.Count];
            
            for (int i = 0; i < _providers.Count; i++)
            {
                SetInterceptorInfo(ref interceptors[i], _providers[i], categoryName);
            }

            return interceptors;
        }
    }
}
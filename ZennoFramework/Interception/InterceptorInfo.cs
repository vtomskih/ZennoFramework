using System;
using System.Collections.Generic;
using ZennoFramework.Interception.Abstractions;
using ZennoFramework.Interception.Configuration;

namespace ZennoFramework.Interception
{
    public struct InterceptorInfo
    {
        public string Name { get; set; }
        public IInterceptor Interceptor { get; set; }
        public List<InterceptorRule> Rules { get; set; }
    }
}
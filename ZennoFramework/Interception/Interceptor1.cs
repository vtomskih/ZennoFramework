using ZennoFramework.Interception.Abstractions;

namespace ZennoFramework.Interception
{
    public class Interceptor : IInterceptor
    {
        public InterceptorInfo[] Interceptors { get; set; }

        public bool IsEnabled
        {
            get => Interceptors != null;
            set { throw new System.NotImplementedException(); }
        }

        public void Execute(object sender)
        {
            var interceptors = Interceptors;
            if (interceptors == null)
            {
                return;
            }

            foreach (var interceptorInfo in interceptors)
            {
                if (interceptorInfo.Interceptor.IsEnabled)
                {
                   interceptorInfo.Interceptor.Execute(sender);
                }
            }
        }
    }
}
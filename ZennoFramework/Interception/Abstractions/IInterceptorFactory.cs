namespace ZennoFramework.Interception.Abstractions
{
    public interface IInterceptorFactory
    {
        IInterceptor CreateInterceptor(string categoryName);
        void AddProvider(IInterceptorProvider provider);
    }
}
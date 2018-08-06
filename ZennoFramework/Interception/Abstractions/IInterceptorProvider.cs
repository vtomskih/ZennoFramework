namespace ZennoFramework.Interception.Abstractions
{
    public interface IInterceptorProvider
    {
        IInterceptor CreateInterceptor(string categoryName);
    }
}
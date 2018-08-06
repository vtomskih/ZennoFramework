namespace ZennoFramework.Interception.Abstractions
{
    public interface IInterceptor
    {
        bool IsEnabled { get; set; }
        void Execute(object sender);
    }
}
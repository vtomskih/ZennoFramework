using ZennoFramework.Interception;
using ZennoFramework.Interception.Abstractions;
using ZennoFramework.Utilities;

namespace ZennoFramework.Infrastructure.Elements
{
    public abstract class ElementContainer<T> : ILoggable, IInterceptable where T : BotContext
    {
        public ElementContainer(T context)
        {
            Context = Check.NotNull(context, nameof(context));
        }

        public T Context { get; }

        SenderInfo IInterceptable.Info
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}
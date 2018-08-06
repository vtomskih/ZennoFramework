using System;
using ZennoLab.CommandCenter;

namespace ZennoFramework.Infrastructure.Elements.Internal
{
    public sealed class Element : Elements.Element
    {
        internal Element(BotContext context, Func<HtmlElement> searchMethod) : base(context, searchMethod)
        {
        }

        // Нужен для работы активатора в ElementCollection`.
        internal Element(BotContext context) : base(context)
        {
        }

        protected override Elements.Element Find() => null;
    }
}
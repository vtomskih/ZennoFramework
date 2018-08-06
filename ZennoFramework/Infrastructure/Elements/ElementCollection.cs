using System;
using ZennoLab.CommandCenter;

namespace ZennoFramework.Infrastructure.Elements
{
    /// <summary>
    /// Представляет коллекцию HTML элементов на веб-странице.
    /// </summary>
    public sealed class ElementCollection : ElementCollection<Internal.Element>
    {
        /// <inheritdoc />
        public ElementCollection(BotContext context, Func<HtmlElementCollection> searchMethod) : base(searchMethod, context)
        {
        }

        /// <inheritdoc />
        protected override ElementCollection Find() => null;
    }
}
using System;
using System.Collections.Generic;
using ZennoFramework.Extensions;
using ZennoFramework.Infrastructure;
using ZennoFramework.Infrastructure.Elements;
using ZennoFramework.Infrastructure.Extensions;

namespace ZennoFramework.Xml.Extensions
{
    public static class BotContextExtensions
    {
        public static void PrepareXPath(this BotContext @this, Dictionary<string, string> keysAndXpaths) =>
            @this.KeysAndXpaths = @this.KeysAndXpaths ?? keysAndXpaths;

        public static Infrastructure.Elements.Element CreateElement(this BotContext @this, string key,
            Infrastructure.Elements.Element parent = null)
        {
            var xpath = GetXPath(@this, key);
            if (parent != null)
                xpath = xpath.StartsWith("/") ? "." + xpath : xpath;

            var element = parent == null
                ? @this.Instance.ActiveTab.WithoutLogging().FindElementByXPath(xpath)
                : parent.WithoutLogging().FindChildByXPath(xpath);
            element.LogName = key;
            return element;
        }

        public static ElementCollection CreateCollection(this BotContext @this, string key,
            Infrastructure.Elements.Element parent = null)
        {
            var xpath = GetXPath(@this, key);
            if (parent != null)
                xpath = xpath.StartsWith("/") ? "." + xpath : xpath;

            var elements = parent == null
                ? @this.Instance.ActiveTab.WithoutLogging().FindElementsByXPath(xpath)
                : parent.WithoutLogging().FindChildrenByXPath(xpath);
            return elements;
        }

        private static string GetXPath(BotContext context, string key)
        {
            if (!context.KeysAndXpaths.ContainsKey(key))
                throw new System.Exception("Не найден XPath для элемента '" + key + "'.");
            if (string.IsNullOrWhiteSpace(context.KeysAndXpaths[key]))
                throw new System.Exception("Не задан XPath для элемента '" + key + "'.");
            return context.KeysAndXpaths[key];
        }
    }
}
using ZennoFramework.Enums;
using ZennoFramework.Infrastructure.Elements;

namespace ZennoFramework.Infrastructure.Extensions
{
    public static class ElementExtensions
    {
        /// <summary>
        /// Возвращает копию текущего элемента, у которой отключено выполнение механизма перехвата
        /// (<see cref="Element.IsInterceptionEnabled"/> = false), заданного в <see cref="BotContext.Interception"/>.
        /// </summary>
        public static Element WithoutInterception(this Element @this)
        {
            var element = @this.IsInterceptionEnabled == OptionState.Disabled ? @this : (Element)@this.Clone();
            element.IsInterceptionEnabled = OptionState.Disabled;
            return element;
        }

        /// <summary>
        /// Возвращает копию текущего элемента, у которой отключен автоматический повторный поиск
        /// (<see cref="Element.IsAutoFindingEnabled"/> = false).
        /// </summary>
        public static Element WithoutRefinding(this Element @this)
        {
            var element = @this.IsAutoFindingEnabled == OptionState.Disabled ? @this : (Element)@this.Clone();
            element.IsAutoFindingEnabled = OptionState.Disabled;
            return element;
        }

        /// <summary>
        /// Возвращает копию текущего элемента, у которой отключено логирование действий
        /// (<see cref="Element.IsLoggingEnabled"/> = false).
        /// </summary>
        public static Element WithoutLogging(this Element @this)
        {
            var element = @this.IsLoggingEnabled == OptionState.Disabled ? @this : (Element)@this.Clone();
            element.IsLoggingEnabled = OptionState.Disabled;
            return element;
        }

        /// <summary>
        /// Возвращает копию текущего элемента, у которой отключена эмуляция ввода текста
        /// (<see cref="Element.IsKeyboardEmulationEnabled"/> = false).
        /// </summary>
        public static Element WithoutKeyboardEmulation(this Element @this)
        {
            var element = @this.IsKeyboardEmulationEnabled == OptionState.Disabled ? @this : (Element)@this.Clone();
            element.IsKeyboardEmulationEnabled = OptionState.Disabled;
            return element;
        }

        /// <summary>
        /// Возвращает копию текущего элемента, у которой отключена эмуляция мыши
        /// (<see cref="Element.IsMouseEmulationEnabled"/> = false).
        /// </summary>
        public static Element WithoutMouseEmulation(this Element @this)
        {
            var element = @this.IsMouseEmulationEnabled == OptionState.Disabled ? @this : (Element)@this.Clone();
            element.IsMouseEmulationEnabled = OptionState.Disabled;
            return element;
        }
    }
}
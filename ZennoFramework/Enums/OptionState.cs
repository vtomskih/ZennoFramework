using ZennoFramework.Infrastructure.Elements;

namespace ZennoFramework.Enums
{
    /// <summary>
    /// Отвечает за настройку использования различных механизмов фреймворка, таких как
    /// <see cref="Element.IsKeyboardEmulationEnabled"/>, <see cref="Element.IsMouseEmulationEnabled"/>...
    /// </summary>
    public enum OptionState
    {
        /// <summary>
        /// По умолчанию - будет использоваться значение из конфигурации контекста <see cref="BotContextConfiguration"/>.
        /// </summary>
        Default,

        /// <summary>
        /// Механизм включен.
        /// </summary>
        Enabled,

        /// <summary>
        /// Механизм выключен.
        /// </summary>
        Disabled
    }
}
using ZennoFramework.Infrastructure.Elements;
using ZennoFramework.Utilities;

namespace ZennoFramework
{
    /// <summary>
    /// Содержит глобальные настройки контекста.
    /// </summary>
    public class BotContextConfiguration
    {
        /// <summary>
        /// Отвечает за автоматическое ожидание загрузки страницы.
        /// По умолчанию <c>true</c>.
        /// </summary>
        public bool IsAutoWaitDownloadingEnabled { get; set; } = true;

        /// <summary>
        /// Отвечает за логирование действий при обращении к методам элементов, табов, инстанса.
        /// По умолчанию <c>true</c>.
        /// </summary>
        public bool IsTraceLoggingEnabled { get; set; } = true;

        /// <summary>
        /// Отвечает за автоматический повторный поиск элементов на странице при обращении к их свойствам и методам.
        /// По умолчанию <c>true</c>.
        /// </summary>
        public bool IsElementAutoFindingEnabled { get; set; } = true;

        /// <summary>
        /// Отвечает за эмуляцию ввода текста.
        /// При включенной эмуляции вызовы метода 
        /// <see cref="Element.SetValue"/> будут автоматически 
        /// заменены на <see cref="Element.SendText"/>.
        /// По умолчанию <c>false</c>.
        /// </summary>
        public bool IsKeyboardEmulationEnabled { get; set; }

        /// <summary>
        /// Задержка между вводимыми символами текста в миллисекундах. По умолчанию от 100 до 300.
        /// </summary>
        public Range KeyboardEmulationLatency { get; set; } = new Range {From = 100, To = 300};

        /// <summary>
        /// Отвечает за автоматическую эмуляцию мыши при выполнении действий над HTML элементами.
        /// По умолчанию <c>false</c>.
        /// </summary>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.1</c>.</remarks>
        public bool IsMouseEmulationEnabled { get; set; }

        /// <summary>
        /// Пауза в миллисекундах между генерируемыми точками, по которым передвигается курсор.
        /// Отвечает за скорость движения мыши. Минимальное значение 0 - очень быстро, чем больше значение паузы, 
        /// тем медленнее движение.
        /// По умолчанию от 3 до 7.
        /// </summary>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.1</c>.</remarks>
        public Range MouseEmulationPause { get; set; } = new Range {From = 3, To = 7};

        /// <summary>
        /// Длина шага по прямой линии между генерируемыми точками.
        /// Отвечает за количество используемых точек и за скорость движения мыши.
        /// Чем больше значение, тем меньше точек и выше скорость движения.
        /// По умолчанию от 4 до 10.
        /// </summary>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.1</c>.</remarks>
        public Range MouseEmulationPointDistanse { get; set; } = new Range {From = 4, To = 10};

    }
}
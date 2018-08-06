using System;
using System.Diagnostics;
using System.Drawing;
using ZennoFramework.Enums;
using ZennoFramework.Infrastructure.Elements;
using ZennoFramework.Infrastructure.Extensions;
using ZennoFramework.Logging;
using ZennoFramework.Logging.Abstractions;
using ZennoFramework.Utilities;
using ZennoLab.CommandCenter;

namespace ZennoFramework.Infrastructure
{
    /// <summary>
    /// Представляет вкладку браузера. Содержит необходимые методы и свойства для работы с экземпляром вкладки.
    /// </summary>
    public class Tab : ICloneable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ZennoLab.CommandCenter.Tab _zennoTab;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BotContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Tab"/>.
        /// </summary>
        /// <param name="tab">Экземпляр <see cref="ZennoLab.CommandCenter.Tab"/>.</param>
        /// <param name="context">Экземпляр контекста <see cref="BotContext"/>.</param>
        public Tab(ZennoLab.CommandCenter.Tab tab, BotContext context)
        {
            _zennoTab = Check.NotNull(tab, nameof(tab));
            _context = Check.NotNull(context, nameof(context));
            _logger = Check.NotNull(context.Logger, nameof(context.Logger));
        }

        /// <summary>
        /// Отвечает за логирование действий при обращении к методам таба. По умолчанию берется 
        /// значение из конфигурации контекста <see cref="BotContextConfiguration.IsTraceLoggingEnabled"/>.
        /// </summary>
        public OptionState IsLoggingEnabled { get; set; }

        /// <summary>
        /// Отвечает за автоматическое ожидание загрузки страницы.
        /// По умолчанию берется значение из конфигурации контекста <see cref="BotContextConfiguration.IsAutoWaitDownloadingEnabled"/>.
        /// </summary>
        public OptionState IsAutoWaitDownloadingEnabled { get; set; }

        /// <summary>
        /// Возвращает экземпляр вкладки <see cref="ZennoLab.CommandCenter.Tab"/>.
        /// </summary>
        public ZennoLab.CommandCenter.Tab ZennoTab => AutoWaitDownloading()._zennoTab;

        /// <summary>
        /// Вкладка имеет состояние Busy.
        /// </summary>
        public bool IsBusy => ZennoTab.IsBusy;

        /// <summary>
        /// Вкладка имеет состояние Interactive.
        /// </summary>
        public bool IsInteractive => ZennoTab.IsInteractive;

        /// <summary>
        /// Вкладка имеет состояние PreComplete.
        /// </summary>
        public bool IsPreComplete => ZennoTab.IsPreComplete;

        /// <summary>
        /// Возвращает текущий домен вкладки.
        /// </summary>
        public string Domain => ZennoTab.Domain;

        /// <summary>
        /// Возвращает текущий основной домен вкладки.
        /// </summary>
        public string MainDomain => ZennoTab.MainDomain;

        /// <summary>
        /// Возвращает dom текст вкладки.
        /// </summary>
        public string DomText => ZennoTab.DomText;

        /// <summary>
        /// Возвращает имя вкладки.
        /// </summary>
        /// <remarks>Доступно с версии ZennoPoster <c>5.3.0.0</c>.</remarks>
        public string Name => ZennoTab.Name;

        /// <summary>
        /// Возвращает заголовок вкладки.
        /// </summary>
        public string Title => ZennoTab.Title;

        /// <summary>
        /// Возвращает URL-адрес страницы текущей вкладки.
        /// </summary>
        public string URL => ZennoTab.URL;

        /// <summary>
        /// Возвращает или задает время ожидания загрузки страницы в секундах.
        /// </summary>
        public int NavigateTimeout
        {
            get => ZennoTab.NavigateTimeout;
            set => ZennoTab.NavigateTimeout = value;
        }

        /// <summary>
        /// Возвращает или задает позицию виртуальной мыши.
        /// </summary>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Point FullEmulationMouseCurrentPosition
        {
            get => ZennoTab.FullEmulationMouseCurrentPosition;
            set => ZennoTab.FullEmulationMouseCurrentPosition = value;
        }

        internal Tab AutoWaitDownloading()
        {
            if ((IsAutoWaitDownloadingEnabled == OptionState.Default && _context.Configuration.IsAutoWaitDownloadingEnabled ||
                IsAutoWaitDownloadingEnabled == OptionState.Enabled) && _zennoTab.IsBusy)
                _zennoTab.WaitDownloading();
            return this;
        }

        /// <summary>
        /// Выполняет переход по указанному адресу.
        /// </summary>
        /// <param name="url">Адрес для перехода.</param>
        /// <param name="referrer">Строковое выражение, результатом которого является заголовок HTTP_REFERER. По умолчанию пуст.</param>
        /// <returns></returns>
        public Tab Navigate(string url, string referrer = "")
        {
            var mousePosition = _zennoTab.FullEmulationMouseCurrentPosition;
            _zennoTab.Navigate(url, referrer);

            this.WithoutLogging().AutoWaitDownloading().FullEmulationMouseSetOptions();
            _zennoTab.FullEmulationMouseCurrentPosition = mousePosition;

            var logMessage = string.IsNullOrWhiteSpace(referrer) ? string.Empty : $", \"{referrer}\"";
            InvokeLogging($"Navigate(\"{url}\"{logMessage})");

            return this;
        }

        /// <summary>
        /// Ожидает загрузки страницы.
        /// </summary>
        /// <returns></returns>
        public Tab WaitDownloading()
        {
            InvokeLogging("WaitDownloading()");
            if (_zennoTab.IsBusy)
                _zennoTab.WaitDownloading();
            return this;
        }

        /// <summary>
        /// Ожидает, пока статус состояния не измененится на Interactive.
        /// </summary>
        /// <returns></returns>
        public Tab WaitInteractive()
        {
            InvokeLogging("WaitInteractive()");
            if (_zennoTab.IsInteractive)
                _zennoTab.WaitInteractive();
            return this;
        }

        /// <summary>
        /// Ожидает, пока статус состояния не измененится на PreComplete.
        /// </summary>
        /// <returns></returns>
        public Tab WaitPreComplete()
        {
            InvokeLogging("WaitPreComplete()");
            if (_zennoTab.IsPreComplete)
                _zennoTab.WaitPreComplete();
            return this;
        }

        /// <summary>
        /// Возвращает изображение предварительного просмотра (скриншот) загруженной страницы.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Размер изображения не превышает 2000 * 2000 пикселей.</remarks>
        public string GetPagePreview()
        {
            InvokeLogging("GetPagePreview()");
            return ZennoTab.GetPagePreview();
        }

        /// <summary>
        /// Возвращает текст страницы, которую видит пользователь.
        /// </summary>
        /// <param name="encoding">Кодировка.</param>
        /// <returns></returns>
        public string GetSourceText(string encoding = "UTF-8")
        {
            InvokeLogging($"GetSourceText(\"{encoding}\")");
            return ZennoTab.GetSourceText(encoding);
        }

        /// <summary>
        /// Закрывает вкладку.
        /// </summary>
        public void Close()
        {
            InvokeLogging("Close()");
            _zennoTab.Close();
        }

        #region FullEmulation

        /// <summary>
        /// Выполняет событие щелчка мыши в текущей точке внутри текущей вкладки.
        /// </summary>
        /// <param name="button">Кнопка мыши. Это может быть "left", "right" или "middle".</param>
        /// <param name="mouseEvent">Событие кнопки мыши. Это может быть "click", "down" или "up".</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Tab FullEmulationMouseClick(string button = "left", string mouseEvent = "click")
        {
            InvokeLogging($"FullEmulationMouseClick(\"{button}\", \"{mouseEvent}\")");
            ZennoTab.FullEmulationMouseClick(button, mouseEvent);
            return this;
        }

        /// <summary>
        /// Выполняет событие перетаскивания мышью из заданных координат в указанные координаты.
        /// </summary>
        /// <param name="fromX">Координата x внутри текущей вкладки от которой начнется перетаскивание (drag).</param>
        /// <param name="fromY">Координата y внутри текущей вкладки от которой начнется перетаскивание (drag).</param>
        /// <param name="toX">Координата x внутри текущей вкладки на которой завершится перетаскивание (drop).</param>
        /// <param name="toY">Координата y внутри текущей вкладки на которой завершится перетаскивание (drop).</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.11.6.0</c>.</remarks>
        public Tab FullEmulationMouseDragAndDrop(int fromX, int fromY, int toX, int toY)
        {
            InvokeLogging($"FullEmulationMouseDragAndDrop({fromX}, {fromY}, {toX}, {toY})");
            ZennoTab.FullEmulationMouseDragAndDrop(fromX, fromY, toX, toY);
            return this;
        }

        /// <summary>
        /// Выполняет событие перетаскивания мышью от указанного html элемента на указанный html элемент.
        /// </summary>
        /// <param name="fromElement">Html элемент от которого начнется перетаскивание (drag).</param>
        /// <param name="toElement">Html элемент на котором завершится перетаскивание (drag).</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.11.6.0</c>.</remarks>
        public Tab FullEmulationMouseDragAndDrop(Element fromElement, Element toElement)
        {
            InvokeLogging($"FullEmulationMouseDragAndDrop({fromElement.LogName}, {toElement.LogName})");
            ZennoTab.FullEmulationMouseDragAndDrop(fromElement.ZennoElement, toElement.ZennoElement);
            return this;
        }

        /// <summary>
        /// Выполняет перемещение виртуальной мыши по заданным координатам из текущего положения.
        /// </summary>
        /// <param name="toX">Координата x внутри области вкладки для перемещения мыши.</param>
        /// <param name="toY">Координата y внутри области вкладки для перемещения мыши.</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Tab FullEmulationMouseMove(int toX, int toY)
        {
            InvokeLogging($"FullEmulationMouseMove({toX}, {toY})");
            ZennoTab.FullEmulationMouseMove(toX, toY);
            return this;
        }

        /// <summary>
        /// Выполняет событие перемещения мыши над указанным html элементом.
        /// </summary>
        /// <param name="element">Html элемент для перемещения мыши.</param>
        /// <param name="sizeOfType">Размер типа.</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Tab FullEmulationMouseMoveAboveHtmlElement(Element element, int sizeOfType)
        {
            InvokeLogging($"FullEmulationMouseMoveAboveHtmlElement({element.LogName}, {sizeOfType})");
            ZennoTab.FullEmulationMouseMoveAboveHtmlElement(element.ZennoElement, sizeOfType);
            return this;
        }

        /// <summary>
        /// Выполняет событие перемещения мыши на указанный html элемент.
        /// </summary>
        /// <param name="element">Html элемент для перемещения мыши.</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Tab FullEmulationMouseMoveToHtmlElement(Element element)
        {
            InvokeLogging($"FullEmulationMouseMoveToHtmlElement({element.LogName})");
            ZennoTab.FullEmulationMouseMoveToHtmlElement(element.ZennoElement);
            return this;
        }

        /// <summary>
        /// Устанавливает параметры эмуляции мыши, метод нужно вызывать после каждого вызова метода <see cref="Navigate"/>.
        /// </summary>
        /// <param name="pause">
        /// Пауза в миллисекундах между генерациями координат.
        /// Если передать <c>null</c>, будет браться значение из настроек контекста <see cref="BotContextConfiguration.MouseEmulationPause"/>.
        /// По умолчанию <c>null</c>.
        /// </param>
        /// <param name="pauseVariance">Дисперсия пауз (pause +- pauseVariance).</param>
        /// <param name="pointDistanse">
        /// Шаг по прямой линии между точками.
        /// Если передать <c>null</c>, будет браться значение из настроек контекста <see cref="BotContextConfiguration.MouseEmulationPointDistanse"/>.
        /// По умолчанию <c>null</c>.
        /// </param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.1</c>.</remarks>
        public Tab FullEmulationMouseSetOptions(int? pause = null, int? pauseVariance = null, int? pointDistanse = null)
        {
            // TODO: добавить еще 3 необязательных параметра для новой версии ZennoPoster + добавить try-catch

            var currentPause = pause ?? _context.Configuration.MouseEmulationPause.Next();
            var currentPauseVariance = pauseVariance ?? 5;
            var currentPointDistanse = pointDistanse ?? _context.Configuration.MouseEmulationPointDistanse.Next();

            InvokeLogging($"FullEmulationMouseSetOptions({pause}, {pauseVariance}, {pointDistanse})");
            ZennoTab.FullEmulationMouseSetOptions(currentPause, currentPauseVariance, currentPointDistanse);
            return this;
        }

        /// <summary>
        /// Настройка границы для прокрутки. Эти параметры устанавливаются в значение по умолчанию после каждого обновления вкладки.
        /// </summary>
        /// <param name="scrollX">Размер границы прокрутки по X.</param>
        /// <param name="scrollY">Размер границы прокрутки по Y.</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.11.6.0</c>.</remarks>
        public Tab FullEmulationMouseSetScrollBorder(int scrollX = 20, int scrollY = 20)
        {
            InvokeLogging($"FullEmulationMouseSetScrollBorder({scrollX}, {scrollY})");
            ZennoTab.FullEmulationMouseSetScrollBorder(scrollX, scrollY);
            return this;
        }

        /// <summary>
        /// Выполняет событие сколла колесиком виртуальной мыши.
        /// </summary>
        /// <param name="deltaX">Размер шага в пикселях вдоль оси X.</param>
        /// <param name="deltaY">Размер шага в пикселях вдоль оси Y.</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.7.0</c>.</remarks>
        public Tab FullEmulationMouseWheel(int deltaX, int deltaY)
        {
            InvokeLogging($"FullEmulationMouseWheel({deltaX}, {deltaY})");
            ZennoTab.FullEmulationMouseWheel(deltaX, deltaY);
            return this;
        }

        #endregion

        #region Find

        /// <summary>
        /// Выполняет поиск элемента по атрибуту и возвращает первое вхождение.
        /// </summary>
        /// <param name="tags">Теги для поиска элемента. Если количество тегов больше одного, они разделяются ";".</param>
        /// <param name="attrName">Имя атрибута.</param>
        /// <param name="attrValue">Значение атрибута.</param>
        /// <param name="searchKind">Этот параметр может принимать следующие значения: "text", "notext" и "regexp".</param>
        /// <param name="number">Номер позиции в коллекции найденных элементов.</param>
        /// <returns></returns>
        public Element FindElementByAttribute(string tags, string attrName, string attrValue, string searchKind,
            int number = 0)
        {
            InvokeLogging(
                $"FindElementByAttribute(\"{tags}\", \"{attrName}\", \"{attrValue}\", \"{searchKind}\", {number})");

            HtmlElement SearchMethod() =>
                ZennoTab.FindElementByAttribute(tags, attrName, attrValue, searchKind, number);

            return new Elements.Internal.Element(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента по атрибуту и возвращает первое вхождение с помощью расширенного селектора чисел.
        /// </summary>
        /// <param name="tags">Теги для поиска элемента. Если количество тегов больше одного, они разделяются ";".</param>
        /// <param name="attrName">Имя атрибута.</param>
        /// <param name="attrValue">Значение атрибута.</param>
        /// <param name="searchKind">Этот параметр может принимать следующие значения: "text", "notext" и "regexp".</param>
        /// <param name="number">Номер позиции. Это может быть число, представленное в виде строки или специального выражения для выбора нужного числа.
        /// Выражение строится по правилам диапазона значений. Например: random1(1,12-15,35-end).</param>
        /// <returns></returns>
        /// <remarks>Доступно с версии ZennoPoster <c>5.9.9.0</c>.</remarks>
        public Element FindElementByAttribute(string tags, string attrName, string attrValue, string searchKind,
            string number)
        {
            InvokeLogging(
                $"FindElementByAttribute(\"{tags}\", \"{attrName}\", \"{attrValue}\", \"{searchKind}\", {number})");

            HtmlElement SearchMethod() =>
                ZennoTab.FindElementByAttribute(tags, attrName, attrValue, searchKind, number);

            return new Elements.Internal.Element(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента по идентификатору и возвращает первое вхождение.
        /// </summary>
        /// <param name="id">Идентификатор, определяющий условие поиска элементов.</param>
        /// <returns></returns>
        public Element FindElementById(string id)
        {
            InvokeLogging($"FindElementById(\"{id}\")");
            HtmlElement SearchMethod() => ZennoTab.FindElementById(id);
            return new Elements.Internal.Element(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента по имени и возвращает первое вхождение.
        /// </summary>
        /// <param name="name">Имя, определяющее условие поиска элементов.</param>
        /// <returns></returns>
        public Element FindElementByName(string name)
        {
            InvokeLogging($"FindElementByName(\"{name}\")");
            HtmlElement SearchMethod() => ZennoTab.FindElementByName(name);
            return new Elements.Internal.Element(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента по тегу и возвращает первое вхождение.
        /// </summary>
        /// <param name="tag">Тэг для поиска элемента.</param>
        /// <param name="number">Номер позиции в коллекции найденных элементов.</param>
        /// <returns></returns>
        public Element FindElementByTag(string tag, int number = 0)
        {
            InvokeLogging($"FindElementByTag(\"{tag}\", {number})");
            HtmlElement SearchMethod() => ZennoTab.FindElementByTag(tag, number);
            return new Elements.Internal.Element(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента по XPath и возвращает первое вхождение.
        /// </summary>
        /// <param name="xpath">XPath для поиска элемента.</param>
        /// <param name="number">Номер позиции в коллекции найденных элементов.</param>
        /// <returns></returns>
        /// <remarks>Доступно с версии ZennoPoster <c>5.3.0.0</c>.</remarks>
        public Element FindElementByXPath(string xpath, int number = 0)
        {
            InvokeLogging($"FindElementByXPath(\"{xpath}\", {number})");
            HtmlElement SearchMethod() => ZennoTab.FindElementByXPath(xpath, number);
            return new Elements.Internal.Element(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов по атрибуту.
        /// </summary>
        /// <param name="tags">Теги для поиска элементов. Если количество тегов больше одного, они разделяются ";".</param>
        /// <param name="attrName">Имя атрибута.</param>
        /// <param name="attrValue">Значение атрибута.</param>
        /// <param name="searchKind">Этот параметр может принимать следующие значения: "text", "notext" и "regexp".</param>
        /// <returns></returns>
        public ElementCollection FindElementsByAttribute(string tags, string attrName, string attrValue,
            string searchKind)
        {
            InvokeLogging($"FindElementsByAttribute(\"{tags}\", \"{attrName}\", \"{attrValue}\", \"{searchKind}\")");

            HtmlElementCollection SearchMethod() =>
                ZennoTab.FindElementsByAttribute(tags, attrName, attrValue, searchKind);

            return new ElementCollection(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор, определяющий условие поиска элементов.</param>
        /// <returns></returns>
        public ElementCollection FindElementsById(string id)
        {
            InvokeLogging($"FindElementsById(\"{id}\")");
            HtmlElementCollection SearchMethod() => ZennoTab.FindElementsById(id);
            return new ElementCollection(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов по имени.
        /// </summary>
        /// <param name="name">Имя, определяющее условие поиска элементов.</param>
        /// <returns></returns>
        public ElementCollection FindElementsByName(string name)
        {
            InvokeLogging($"FindElementsByName(\"{name}\")");
            HtmlElementCollection SearchMethod() => ZennoTab.FindElementsByName(name);
            return new ElementCollection(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов по тегам.
        /// </summary>
        /// <param name="tags">Теги для поиска элементов. Если количество тегов больше одного, они разделяются ";".</param>
        /// <returns></returns>
        public ElementCollection FindElementsByTags(string tags)
        {
            InvokeLogging($"FindElementsByTags(\"{tags}\")");
            HtmlElementCollection SearchMethod() => ZennoTab.FindElementsByTags(tags);
            return new ElementCollection(_context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов по XPath.
        /// </summary>
        /// <param name="xpath">XPath для поиска элементов.</param>
        /// <returns></returns>
        /// <remarks>Доступно с версии ZennoPoster <c>5.3.0.0</c>.</remarks>
        public ElementCollection FindElementsByXPath(string xpath)
        {
            InvokeLogging($"FindElementsByXPath(\"{xpath}\")");
            HtmlElementCollection SearchMethod() => ZennoTab.FindElementsByXPath(xpath);
            return new ElementCollection(_context, SearchMethod);
        }

        #endregion

        #region WaitFor

        /// <summary>
        /// Ожидает, пока на странице не будет найден элемент.
        /// </summary>
        /// <param name="elementForSearch">Элемент для поиска.</param>
        /// <param name="timeout">Длительность поиска в миллисекундах. По умолчанию 5000.</param>
        /// <returns></returns>
        public bool WaitFor(Element elementForSearch, int timeout = 5000)
        {
            bool Predicate() => !elementForSearch.IsHidden;
            var result = this.WithoutLogging().WaitFor(Predicate, timeout);

            InvokeLogging($"WaitFor({elementForSearch.LogName}, {timeout}) : {result}");
            return result;
        }

        /// <summary>
        /// Ожидает, пока не будет выполнено выражение предиката.
        /// </summary>
        /// <param name="predicate">Условное выражение.</param>
        /// <param name="timeout">Длительность проверки выражения в миллисекундах.</param>
        /// <returns></returns>
        public bool WaitFor(Func<bool> predicate, int timeout = 5000)
        {
            var loggingState = _context.Configuration.IsTraceLoggingEnabled;
            _context.Configuration.IsTraceLoggingEnabled = false;

            var result = Waiter.WaitFor(predicate, timeout);

            _context.Configuration.IsTraceLoggingEnabled = loggingState;
            InvokeLogging($"WaitFor(predicate, {timeout}) : {result}");
            return result;
        }

        #endregion

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }

        private void InvokeLogging(string message, params object[] args)
        {
            if (IsLoggingEnabled == OptionState.Default && _context.Configuration.IsTraceLoggingEnabled ||
                IsLoggingEnabled == OptionState.Enabled)
                _logger.Trace($"Tab.{message}", args);
        }
    }
}
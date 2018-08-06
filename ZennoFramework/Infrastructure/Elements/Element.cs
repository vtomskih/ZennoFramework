using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ZennoFramework.Enums;
using ZennoFramework.Exceptions;
using ZennoFramework.Extensions;
using ZennoFramework.Infrastructure.Elements.Internal;
using ZennoFramework.Infrastructure.Extensions;
using ZennoFramework.Interception;
using ZennoFramework.Interception.Abstractions;
using ZennoFramework.Logging;
using ZennoFramework.Logging.Abstractions;
using ZennoFramework.Utilities;
using ZennoLab.CommandCenter;

namespace ZennoFramework.Infrastructure.Elements
{
    /// <summary>
    /// Представляет HTML элемент на веб-странице. Содержит методы для поиска html элементов, получения или установки значений.
    /// </summary>
    public abstract class Element : ILoggable, IInterceptable, ICloneable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tab _parentTab;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HtmlElement _he;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Func<HtmlElement> SearchMethod { get; set; }

        internal Element(BotContext context, Func<HtmlElement> searchMethod) : this(context)
        {
            SearchMethod = Check.NotNull(searchMethod, nameof(searchMethod));
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Element"/>.
        /// </summary>
        /// <param name="context">Экземпляр контекста <see cref="BotContext"/>.</param>
        protected Element(BotContext context)
        {
            Context = Check.NotNull(context, nameof(context));
            _logger = Check.NotNull(context.Logger, nameof(context.Logger));

            var info = StackTraceHelper.GetInfo();
            ((IInterceptable) this).Info = info;
            LogName = info.FullName;
        }

        SenderInfo IInterceptable.Info { get; set; }

        /// <summary>
        /// Контекст бота.
        /// </summary>
        protected BotContext Context { get; }

        /// <summary>
        /// Имя элемента, отображаемое в Trace логах.
        /// По умолчанию задается в соответствии с местом в коде где элемент был инициализирован.
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// Отвечает за выполнение механизма перехвата для текущего элемента при обращении к его методам.
        /// По умолчанию берется значение из настроек <see cref="BotContext.Interception"/>.
        /// </summary>
        public OptionState IsInterceptionEnabled { get; set; }

        /// <summary>
        /// Отвечает за автоматический повторный поиск элемента на странице при обращении к его свойствам и методам.
        /// По умолчанию берется значение из конфигурации контекста <see cref="BotContextConfiguration.IsElementAutoFindingEnabled"/>.
        /// </summary>
        public OptionState IsAutoFindingEnabled { get; set; }

        /// <summary>
        /// Отвечает за логирование действий с текущим элементом при обращении к его методам. По умолчанию берется 
        /// значение из конфигурации контекста <see cref="BotContextConfiguration.IsTraceLoggingEnabled"/>.
        /// </summary>
        public OptionState IsLoggingEnabled { get; set; }

        /// <summary>
        /// Отвечает за эмуляцию ввода текста. При включенной эмуляции вызовы метода <see cref="Element.SetValue"/>
        /// будут автоматически заменены на <see cref="Element.SendText"/>.
        /// По умолчанию берется значение из конфигурации контекста <see cref="BotContextConfiguration.IsKeyboardEmulationEnabled"/>.
        /// </summary>
        public OptionState IsKeyboardEmulationEnabled { get; set; }

        /// <summary>
        /// Отвечает за эмуляцию виртуальной мыши. При включенной эмуляции действия над элементом будут совершаться совместно с эмуляцией.
        /// По умолчанию берется значение из конфигурации контекста <see cref="BotContextConfiguration.IsMouseEmulationEnabled"/>.
        /// </summary>
        public OptionState IsMouseEmulationEnabled { get; set; }

        /// <summary>
        /// Возвращает экземпляр html элемента <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement ZennoElement
        {
            get
            {
                _parentTab?.AutoWaitDownloading();

                var autoFindingEnabled =
                    IsAutoFindingEnabled == OptionState.Default && Context.Configuration.IsElementAutoFindingEnabled ||
                    IsAutoFindingEnabled == OptionState.Enabled;
                var he = autoFindingEnabled || _he == null ? ReFind()._he : _he;
                _parentTab = _parentTab ?? _he.ParentTab.ToExtended(Context);
                _parentTab.AutoWaitDownloading();
                return he;
            }
        }

        /// <summary>
        /// Вкладка, на которой был найден элемент.
        /// </summary>
        public Tab ParentTab => _parentTab ?? (_parentTab = ZennoElement.ParentTab.ToExtended(Context));

        /// <summary>
        /// Возвращает <c>true</c>, если элемент не найден.
        /// </summary>
        public bool IsNull => ZennoElement.IsNull;

        /// <summary>
        /// Возвращает <c>true</c>, если элемент не найден (<see cref="IsNull"/>) или 
        /// скрыт (установлено свойство стиля <c>display="none"</c>).
        /// </summary>
        public bool IsHidden => CheckHidden();

        /// <summary>
        /// Высота html-элемента в пикселях.
        /// </summary>
        public int Height => ZennoElement.Height;

        /// <summary>
        /// Ширина html-элемента в пикселях.
        /// </summary>
        public int Width => ZennoElement.Width;

        /// <summary>
        /// Html внутри тега элемента.
        /// </summary>
        public string InnerHtml => ZennoElement.InnerHtml;

        /// <summary>
        /// Текст внутри тега элемента.
        /// </summary>
        /// <remarks>Доступно с версии ZennoPoster <c>5.9.7.1</c>.</remarks>
        public string InnerText => ZennoElement.InnerText;

        /// <summary>
        /// Текст dom элемента html.
        /// </summary>
        public string OuterHtml => ZennoElement.OuterHtml;

        private bool CheckHidden()
        {
            if (IsNull)
                return true;

            var element = this.WithoutLogging().WithoutRefinding().WithoutInterception();
            return element.Width <= 0 || element.GetStylePropertyValue("display") == "none";
        }

        /// <summary>
        /// Выполняет поиск элемента на странице каждый раз, когда вызывается метод <see cref="ReFind"/>.
        /// </summary>  
        /// <returns></returns>
        protected abstract Element Find();

        /// <summary>
        /// Выполняет повторный поиск текущего элемента на странице.
        /// <para>
        /// Данный метод вызывается автоматически, если в настройках контекста включен механизм автоматического поиска елементов
        /// <see cref="BotContextConfiguration.IsElementAutoFindingEnabled"/> (установлено в <c>true</c>) и 
        /// свойство <see cref="IsAutoFindingEnabled"/> у текущего элемента не установлено в <c>false</c>.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public Element ReFind()
        {
            var element = this.WithoutLogging();
            _he = SearchMethod != null ? element.SearchMethod() : element.Find().ZennoElement ?? _he;
            return this;
        }

        /// <summary>
        /// Выбрасывает исключение, если элемент не найден на странице (<see cref="IsNull"/> == <c>true</c>)
        /// или скрыт (<see cref="IsHidden"/> == <c>true</c>).
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ElementNotFoundException">Элемент не найден: <see cref="LogName"/>.</exception>
        /// <exception cref="ElementHiddenException">Элемент скрыт: <see cref="LogName"/>.</exception>
        public Element ThrowIfNullOrHidden()
        {
            return ThrowIfNull().WithoutRefinding().ThrowIfHidden();
        }

        /// <summary>
        /// Выбрасывает исключение, если элемент не найден на странице (<see cref="IsNull"/> == <c>true</c>).
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ElementNotFoundException">Элемент не найден: <see cref="LogName"/>.</exception>
        public Element ThrowIfNull()
        {
            return IsNull ? throw new ElementNotFoundException($"Элемент не найден: {LogName}.") : this;
        }

        /// <summary>
        /// Выбрасывает исключение, если элемент скрыт - не отображается на странице (<see cref="IsHidden"/> == <c>true</c>).
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ElementHiddenException">Элемент скрыт: <see cref="LogName"/>.</exception>
        public Element ThrowIfHidden()
        {
            return IsHidden ? throw new ElementHiddenException($"Элемент скрыт: {LogName}.") : this;
        }

        /// <summary>
        /// Выполняет событие клика по элементу.
        /// </summary>
        /// <returns></returns>
        public Element Click()
        {
            InvokeInterception();

            var state = new ElementState {IsNullIn = IsNull};
            var element = this.WithoutRefinding();
            state.IsHiddenIn = element.IsHidden;

            InvokeMouseMove(true);
            if (IsMouseEmulationEnabled == OptionState.Default && !Context.Configuration.IsMouseEmulationEnabled ||
                IsMouseEmulationEnabled == OptionState.Disabled)
                _he.Click();

            InvokeLogging("Click()", state);

            InvokeInterception();

            return this;
        }

        /// <summary>
        /// Возвращает значение атрибута.
        /// </summary>
        /// <param name="name">Имя атрибута.</param>
        /// <returns></returns>
        public string GetAttribute(string name)
        {
            var result = ZennoElement.GetAttribute(name);
            InvokeLogging($"GetAttribute(\"{name}\")");
            InvokeInterception();
            return result;
        }

        /// <summary>
        /// Устанавливает значение атрибута.
        /// </summary>
        /// <param name="name">Имя атрибута.</param>
        /// <param name="value">Устанавливаемое значение.</param>
        /// <returns></returns>
        public Element SetAttribute(string name, string value)
        {
            InvokeInterception();

            var state = new ElementState {IsNullIn = IsNull};
            var element = this.WithoutRefinding();
            state.IsHiddenIn = element.IsHidden;

            element.InvokeMouseMove();
            _he.SetAttribute(name, value);

            InvokeLogging($"SetAttribute(\"{name}\", \"{value}\")", state);

            InvokeInterception();
            return this;
        }

        /// <summary>
        /// Возвращает текст свойства style.
        /// </summary>
        /// <param name="property">Имя свойства style.</param>
        /// <returns></returns>
        public string GetStylePropertyValue(string property)
        {
            var style = this.WithoutLogging().GetAttribute("style").Replace(" ", "");
            var pattern =
                $@"(?<={property.Trim().ToLower()}="")[^.\""]+(?="")|(?<={property.Trim().ToLower()}=')[^.\']+(?=')";
            InvokeLogging($"GetStylePropertyValue(\"{property}\")");
            return Regex.Match(style, pattern).Value;
        }

        /// <summary>
        /// Возвращает значение элемента.
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            InvokeInterception();
            var result = ZennoElement.GetValue();
            InvokeLogging("GetValue()");
            return result;
        }

        /// <summary>
        /// Устанавливает значение элемента.
        /// </summary>
        /// <param name="value">Устанавливаемое значение.</param>
        /// <param name="emulation">Уровень эмуляции. Может быть: "None", "Middle" или "Full".</param>
        /// <param name="useSelectedItems"><c>true</c>, если необходимо использовать автоматическое заполнение стандартных полей "select"; в противном случае <c>false</c>.</param>
        /// <param name="append"><c>true</c>, если требуется добавить значение к существующему содержимому; в противном случае <c>false</c>.
        /// <para>Аргумент доступен с версии ZennoPoster <c>5.16.0.0</c>.</para>
        /// </param>
        /// <returns></returns>
        /// <remarks>Использование аргумента <c>append</c> доступно с версии ZennoPoster <c>5.16.0.0</c>.</remarks>
        public Element SetValue(string value, string emulation = "None", bool useSelectedItems = false,
            bool append = false)
        {
            var useKeyboardEmulation =
                IsKeyboardEmulationEnabled == OptionState.Default && Context.Configuration.IsKeyboardEmulationEnabled ||
                IsKeyboardEmulationEnabled == OptionState.Enabled;

            var inputType = useKeyboardEmulation ? GetInputType() : string.Empty;

            if (useKeyboardEmulation && inputType == "text" || inputType == "password" || inputType == "textarea")
            {
                SendText(value);
            }
            else
            {
                InvokeInterception();

                var state = new ElementState {IsNullIn = IsNull};
                var element = this.WithoutRefinding();
                state.IsHiddenIn = element.IsHidden;

                element.InvokeMouseMove();

                dynamic he = _he;
                try
                {
                    // TODO: выкидывать исключение, если append = true и версия ZP не соответствует.
                    he.SetValue(value, emulation, useSelectedItems, append);
                }
                catch (System.Exception)
                {
                    he.SetValue(value, emulation, useSelectedItems);
                }

                InvokeLogging($"SetValue(\"{value}\")", state);

                InvokeInterception();
            }

            return this;
        }

        private string GetInputType()
        {
            return this.WithoutLogging().GetAttribute("type").Trim().ToLower();
        }

        /// <summary>
        /// Эмулирует ввод с клавиатуры.
        /// </summary>
        /// <param name="text">Текст для ввода.</param>
        /// <param name="latency">Задержка между вводимыми символами текста. 
        /// По умолчанию генерируется в соответствии с параметрами <see cref="BotContextConfiguration.KeyboardEmulationLatency"/>.</param>
        /// <returns></returns>
        public Element SendText(string text, int? latency = null)
        {
            InvokeInterception();

            var state = new ElementState {IsNullIn = IsNull};
            var element = this.WithoutRefinding();
            state.IsHiddenIn = element.IsHidden;

            element.InvokeMouseMove(true);

            _he.RiseEvent("focus", "None");
            latency = latency ?? Context.Configuration.KeyboardEmulationLatency.Next();
            Context.Instance.WithoutLogging().SendText(text, latency);

            InvokeLogging($"SendText(\"{text}\", {latency})", state);

            InvokeInterception();
            return this;
        }

        /// <summary>
        /// Выполняет указанное событие для элемента.
        /// </summary>
        /// <param name="eventName">Название события.</param>
        /// <param name="emulation">Уровень эмуляции. Может быть: "None", "Middle" или "Full".</param>
        /// <returns></returns>
        public Element RiseEvent(string eventName, string emulation = "Full")
        {
            InvokeInterception();

            var state = new ElementState {IsNullIn = IsNull};
            var element = this.WithoutRefinding();
            state.IsHiddenIn = element.IsHidden;

            element.InvokeMouseMove();
            _he.RiseEvent(eventName, emulation);

            InvokeLogging($"RiseEvent(\"{eventName}\", \"{emulation}\")", state);

            InvokeInterception();
            return this;
        }

        /// <summary>
        /// Выполняет прокрутку вкладки до текущего элемента.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Доступно с версии ZennoPoster <c>5.4.0.0</c>.</remarks>
        public Element ScrollIntoView()
        {
            InvokeInterception();
            ZennoElement.ScrollIntoView();
            InvokeLogging("ScrollIntoView()");
            return this;
        }

        /// <summary>
        /// Выполняет рендеринг в растровое изображение (скриншот элемента).
        /// </summary>
        /// <param name="isImage"><c>true</c>, если тег этого элемента "img", иначе <c>false</c>.</param>
        /// <returns></returns>
        public string DrawToBitmap(bool isImage)
        {
            InvokeInterception();

            // Если возникнет исключение, что метод не найден, добавить вызов с двумя параметрами.
            // Второй параметр string hash появился в ZennoPoster 5.7.
            var result = ZennoElement.DrawToBitmap(isImage);
            InvokeLogging($"DrawToBitmap({isImage})");
            return result;
        }

        #region FullEmulation

        /// <summary>
        /// Выполняет событие щелчка мыши в текущей точке внутри текущей вкладки.
        /// </summary>
        /// <param name="button">Кнопка мыши. Это может быть "left", "right" или "middle".</param>
        /// <param name="mouseEvent">Событие кнопки мыши. Это может быть "click", "down" или "up".</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Element FullEmulationMouseClick(string button = "left", string mouseEvent = "click")
        {
            var state = new ElementState {IsNullIn = IsNull};
            var element = this.WithoutRefinding();
            state.IsHiddenIn = element.IsHidden;

            var tab = ParentTab.WithoutLogging().WithoutWaitDownloading();
            tab.FullEmulationMouseMoveToHtmlElement(element);
            tab.FullEmulationMouseClick(button, mouseEvent);

            InvokeLogging($"FullEmulationMouseClick(\"{button}\", \"{mouseEvent}\")", state);
            return this;
        }

        /// <summary>
        /// Выполняет событие перетаскивания мышью из заданных координат в указанные координаты.
        /// </summary>
        /// <param name="toX">Координата x внутри текущей вкладки на которой завершится перетаскивание (drop).</param>
        /// <param name="toY">Координата y внутри текущей вкладки на которой завершится перетаскивание (drop).</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.11.6.0</c>.</remarks>
        public Element FullEmulationMouseDragAndDrop(int toX, int toY)
        {
            var displacement = ZennoElement.DisplacementInTabWindow;
            var tab = ParentTab.WithoutLogging().WithoutWaitDownloading();
            tab.FullEmulationMouseMoveToHtmlElement(this);
            tab.FullEmulationMouseDragAndDrop(displacement.X, displacement.Y, toX, toY);

            InvokeLogging($"FullEmulationMouseDragAndDrop({toX}, {toY})");
            return this;
        }

        /// <summary>
        /// Выполняет событие перетаскивания мышью от указанного html элемента на указанный html элемент.
        /// </summary>
        /// <param name="toElement">Html элемент на котором завершится перетаскивание (drag).</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.11.6.0</c>.</remarks>
        public Element FullEmulationMouseDragAndDrop(Element toElement)
        {
            var displacement = ZennoElement.DisplacementInTabWindow;
            var tab = ParentTab.WithoutLogging().WithoutWaitDownloading();
            tab.FullEmulationMouseMove(displacement.X, displacement.Y);
            tab.FullEmulationMouseDragAndDrop(this.WithoutRefinding(), toElement);

            InvokeLogging($"FullEmulationMouseDragAndDrop({toElement.LogName})");
            return this;
        }

        /// <summary>
        /// Выполняет перемещение виртуальной мыши к текущему элементу.
        /// </summary>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Element FullEmulationMouseMove()
        {
            var tab = ParentTab.WithoutLogging().WithoutWaitDownloading();
            tab.FullEmulationMouseMoveToHtmlElement(this);

            InvokeLogging("FullEmulationMouseMove()");
            return this;
        }

        /// <summary>
        /// Выполняет событие перемещения мыши над указанным html элементом.
        /// </summary>
        /// <param name="sizeOfType">Размер отступов/примерный размер шрифта "читаемого" текста.</param>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Element FullEmulationMouseMoveAboveHtmlElement(int sizeOfType)
        {
            var displacement = ZennoElement.DisplacementInTabWindow;
            var tab = ParentTab.WithoutLogging().WithoutWaitDownloading();
            tab.FullEmulationMouseMove(displacement.X, displacement.Y);
            tab.FullEmulationMouseMoveAboveHtmlElement(this, sizeOfType);

            InvokeLogging($"FullEmulationMouseMoveAboveHtmlElement({sizeOfType})");
            return this;
        }

        private void InvokeMouseMove(bool click = false)
        {
            if (IsMouseEmulationEnabled == OptionState.Default && !Context.Configuration.IsMouseEmulationEnabled ||
                IsMouseEmulationEnabled == OptionState.Disabled)
                return;

            ParentTab.WithoutLogging().WithoutWaitDownloading().FullEmulationMouseSetOptions();
            var element = this.WithoutLogging().WithoutInterception();

            if (click)
                element.FullEmulationMouseClick();
            else
                element.FullEmulationMouseMove();
        }

        #endregion

        #region Find

        /// <summary>
        /// Выполняет поиск элемента среди дочерних элементов по атрибуту и возвращает первое вхождение.
        /// </summary>
        /// <param name="tags">Теги для поиска элемента. Если количество тегов больше одного, они разделяются ";".</param>
        /// <param name="attrName">Имя атрибута.</param>
        /// <param name="attrValue">Значение атрибута.</param>
        /// <param name="searchKind">Этот параметр может принимать следующие значения: "text", "notext" и "regexp".</param>
        /// <param name="number">Номер позиции в коллекции найденных элементов.</param>
        /// <returns></returns>
        public Element FindChildByAttribute(string tags, string attrName, string attrValue, string searchKind,
            int number = 0)
        {
            InvokeLogging(
                $"FindChildByAttribute(\"{tags}\", \"{attrName}\", \"{attrValue}\", \"{searchKind}\", {number})");

            HtmlElement SearchMethod() =>
                ZennoElement.FindChildByAttribute(tags, attrName, attrValue, searchKind, number);

            return new Internal.Element(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента среди дочерних элементов по идентификатору и возвращает первое вхождение.
        /// </summary>
        /// <param name="id">Идентификатор, определяющий условие поиска элементов.</param>
        /// <returns></returns>
        public Element FindChildById(string id)
        {
            InvokeLogging($"FindChildById(\"{id}\")");
            HtmlElement SearchMethod() => ZennoElement.FindChildById(id);
            return new Internal.Element(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента среди дочерних элементов по имени и возвращает первое вхождение.
        /// </summary>
        /// <param name="name">Имя, определяющее условие поиска элементов.</param>
        /// <returns></returns>
        public Element FindChildByName(string name)
        {
            InvokeLogging($"FindChildByName(\"{name}\")");
            HtmlElement SearchMethod() => ZennoElement.FindChildByName(name);
            return new Internal.Element(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента среди дочерних элементов по тегу и возвращает первое вхождение.
        /// </summary>
        /// <param name="tag">Тэг для поиска элемента.</param>
        /// <param name="number">Номер позиции в коллекции найденных элементов.</param>
        /// <returns></returns>
        public Element FindChildByTag(string tag, int number = 0)
        {
            InvokeLogging($"FindChildByTag(\"{tag}\", {number})");
            HtmlElement SearchMethod() => ZennoElement.FindChildByTag(tag, number);
            return new Internal.Element(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск элемента среди дочерних элементов по XPath.
        /// </summary>
        /// <param name="xpath">XPath для поиска элемента.</param>
        /// <param name="number">Номер позиции в коллекции найденных элементов.</param>
        /// <returns></returns>
        /// <remarks>Доступно с версии ZennoPoster <c>5.3.0.0</c>.</remarks>
        public Element FindChildByXPath(string xpath, int number = 0)
        {
            InvokeLogging($"FindChildByXPath(\"{xpath}\", {number})");
            HtmlElement SearchMethod() => ZennoElement.FindChildByXPath(xpath, number);
            return new Internal.Element(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов среди дочерних элементов по атрибуту.
        /// </summary>
        /// <param name="tags">Теги для поиска элементов. Если количество тегов больше одного, они разделяются ";".</param>
        /// <param name="attrName">Имя атрибута.</param>
        /// <param name="attrValue">Значение атрибута.</param>
        /// <param name="searchKind">Этот параметр может принимать следующие значения: "text", "notext" и "regexp".</param>
        /// <returns></returns>
        public ElementCollection FindChildrenByAttribute(string tags, string attrName, string attrValue,
            string searchKind)
        {
            InvokeLogging($"FindChildrenByAttribute(\"{tags}\", \"{attrName}\", \"{attrValue}\", \"{searchKind}\")");

            HtmlElementCollection SearchMethod() =>
                ZennoElement.FindChildrenByAttribute(tags, attrName, attrValue, searchKind);

            return new ElementCollection(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов среди дочерних элементов по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор, определяющий условие поиска элементов.</param>
        /// <returns></returns>
        public ElementCollection FindChildrenById(string id)
        {
            InvokeLogging($"FindChildrenById(\"{id}\")");
            HtmlElementCollection SearchMethod() => ZennoElement.FindChildrenById(id);
            return new ElementCollection(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов среди дочерних элементов по имени.
        /// </summary>
        /// <param name="name">Имя, определяющее условие поиска элементов.</param>
        /// <returns></returns>
        public ElementCollection FindChildrenByName(string name)
        {
            InvokeLogging($"FindChildrenByName(\"{name}\")");
            HtmlElementCollection SearchMethod() => ZennoElement.FindChildrenByName(name);
            return new ElementCollection(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов среди дочерних элементов по тегам.
        /// </summary>
        /// <param name="tags">Теги для поиска элементов. Если количество тегов больше одного, они разделяются ";".</param>
        /// <returns></returns>
        public ElementCollection FindChildrenByTags(string tags)
        {
            InvokeLogging($"FindChildrenByTags(\"{tags}\")");
            HtmlElementCollection SearchMethod() => ZennoElement.FindChildrenByTags(tags);
            return new ElementCollection(Context, SearchMethod);
        }

        /// <summary>
        /// Выполняет поиск всех элементов среди дочерних элементов по XPath.
        /// </summary>
        /// <param name="xpath">XPath для поиска элементов.</param>
        /// <returns></returns>
        /// <remarks>Доступно с версии ZennoPoster <c>5.3.0.0</c>.</remarks>
        public ElementCollection FindChildrenByXPath(string xpath)
        {
            InvokeLogging($"FindChildrenByXPath(\"{xpath}\")");
            HtmlElementCollection SearchMethod() => ZennoElement.FindChildrenByXPath(xpath);
            return new ElementCollection(Context, SearchMethod);
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
            var loggingState = Context.Configuration.IsTraceLoggingEnabled;
            Context.Configuration.IsTraceLoggingEnabled = false;

            var result = Waiter.WaitFor(predicate, timeout);

            Context.Configuration.IsTraceLoggingEnabled = loggingState;
            InvokeLogging($"WaitFor(predicate, {timeout}) : {result}");
            return result;
        }

        #endregion

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }

        private void InvokeLogging(string message, ElementState state = null)
        {
            if (IsLoggingEnabled == OptionState.Default && !Context.Configuration.IsTraceLoggingEnabled ||
                IsLoggingEnabled == OptionState.Disabled)
                return;

            if (state != null)
            {
                state.IsNullOut = IsNull;
                state.IsHiddenOut = this.WithoutRefinding().IsHidden;
            }

            var elementState = state == null
                ? $" : [IsNull({this.WithoutRefinding().IsNull}), IsHidden({this.WithoutRefinding().IsHidden})]"
                : $" : [IsNull({state.IsNullIn}, {state.IsNullOut}), IsHidden({state.IsNullIn}, {state.IsNullOut})]";
            _logger.Trace($"{LogName}.{message}{elementState}");
        }

        private void InvokeInterception()
        {
            if (IsInterceptionEnabled == OptionState.Default && Context.Interception.IsEnabled ||
                IsInterceptionEnabled == OptionState.Enabled)
                Context.Interception.Execute(this);
        }
    }
}
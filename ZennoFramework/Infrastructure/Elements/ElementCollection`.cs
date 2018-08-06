using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ZennoFramework.Enums;
using ZennoFramework.Interception;
using ZennoFramework.Interception.Abstractions;
using ZennoFramework.Logging;
using ZennoFramework.Logging.Abstractions;
using ZennoFramework.Utilities;
using ZennoLab.CommandCenter;

namespace ZennoFramework.Infrastructure.Elements
{
    /// <summary>
    /// Представляет коллекцию HTML элементов на веб-странице.
    /// </summary>
    /// <typeparam name="TElement">Элемент коллекции, является типом <see cref="Element"/> или его потомком.</typeparam>
    public abstract class ElementCollection<TElement> : ILoggable, IInterceptable, ICloneable, IEnumerable<TElement>
        where TElement : class
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly object[] _itemCtorParams;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Type _itemType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Func<HtmlElementCollection> _searchMethod;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HtmlElementCollection _hec;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<TElement> _elements;

        internal ElementCollection(Func<HtmlElementCollection> searchMethod, BotContext context) : this(context)
        {
            _searchMethod = Check.NotNull(searchMethod, nameof(searchMethod));
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ElementCollection{TElement}"/>.
        /// </summary>
        /// <param name="context">Экземпляр контекста <see cref="BotContext"/>.</param>
        protected ElementCollection(BotContext context)
        {
            Context = Check.NotNull(context, nameof(context));
            _logger = Check.NotNull(Context.Logger, nameof(Context.Logger));

            _itemType = typeof(TElement);
            Check.ElementCollectionItem(_itemType, out var itemCtorParametersCount);

            _itemCtorParams = new object[itemCtorParametersCount];
            _itemCtorParams[0] = Context;

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
        /// Имя коллекции, отображаемое в Trace логах.
        /// По умолчанию задается в соответствии с местом в коде где коллекция была инициализирована.
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// Отвечает за логирование действий при поиске элементов. По умолчанию берется 
        /// значение из конфигурации контекста <see cref="BotContextConfiguration.IsTraceLoggingEnabled"/>.
        /// </summary>
        public OptionState IsLoggingEnabled { get; set; }

        /// <summary>
        /// Возвращает объект эелемента <see cref="Element"/> по его индексу из списка <see cref="Elements"/>.
        /// </summary>
        /// <param name="index">Индекс элемента в списке <see cref="Elements"/>.</param>
        public TElement this[int index] => Elements[index];

        /// <summary>
        /// Представляет список html елементов, найденных на странице при последнем выполнении поиска.
        /// </summary>
        public List<TElement> Elements => _elements ?? ReFind()._elements;

        /// <summary>
        /// Получает число htnl элементов, найденных на странице при последнем выполнении поиска.
        /// </summary>
        public int Count => Elements.Count;

        /// <summary>
        /// Возвращает экземпляр коллекции элементов <see cref="HtmlElementCollection"/>.
        /// </summary>
        public HtmlElementCollection ZennoElements => _hec ?? ReFind()._hec;

        /// <summary>
        /// Выполняет поиск элементов на странице каждый раз, когда вызывается метод <see cref="ReFind" />.
        /// </summary>
        /// <returns></returns>
        protected abstract ElementCollection Find();

        /// <summary>
        /// Выполняет повторный поиск элементов на странице.
        /// </summary>
        /// <returns></returns>
        public ElementCollection<TElement> ReFind()
        {
            _elements = _elements ?? new List<TElement>();
            _elements.Clear();
            _hec = _searchMethod != null ? _searchMethod() : Find().WithoutLogging().ZennoElements ?? _hec;

            for (var i = 0; i < _hec.Count - 1; i++)
            {
                const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                var element = (TElement) Activator.CreateInstance(typeof(TElement), flags, null, _itemCtorParams, null);
                var baseElement = element as Element;
                baseElement.SearchMethod = () => _hec.ElementAt(i);
                baseElement.IsAutoFindingEnabled = OptionState.Disabled;
                baseElement.LogName = $"{LogName}[{i}]";
                _elements.Add(element);
            }

            InvokeLogging($"ReFind() : найдено {Count} элементов");
            return this;
        }

        /// <summary>
        /// Возвращает копию текущей коллекции элементов, у которой отключено логирование действий
        /// (<see cref="IsLoggingEnabled"/> = false).
        /// </summary>
        public ElementCollection<TElement> WithoutLogging()
        {
            var collection = IsLoggingEnabled == OptionState.Disabled ? this : (ElementCollection<TElement>) Clone();
            collection.IsLoggingEnabled = OptionState.Disabled;
            return collection;
        }

        /// <inheritdoc />
        public IEnumerator<TElement> GetEnumerator() => Elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }

        private void InvokeLogging(string message)
        {
            if (IsLoggingEnabled == OptionState.Default && Context.Configuration.IsTraceLoggingEnabled ||
                IsLoggingEnabled == OptionState.Enabled)
                _logger.Trace($"{LogName}.{message}");
        }
    }
}
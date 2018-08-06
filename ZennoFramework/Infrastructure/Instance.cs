using System;
using System.Diagnostics;
using ZennoFramework.Enums;
using ZennoFramework.Extensions;
using ZennoFramework.Logging;
using ZennoFramework.Logging.Abstractions;
using ZennoFramework.Utilities;

namespace ZennoFramework.Infrastructure
{
    /// <summary>
    /// Представляет инстанс браузера. Содержит необходимые методы и свойства для работы с инстансом.
    /// </summary>
    public class Instance : ICloneable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BotContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Instance"/>.
        /// </summary>
        /// <param name="instance">Экземпляр <see cref="ZennoLab.CommandCenter.Instance"/>.</param>
        /// <param name="context">Экземпляр контекста <see cref="BotContext"/>.</param>
        public Instance(ZennoLab.CommandCenter.Instance instance, BotContext context)
        {
            ZennoInstance = Check.NotNull(instance, nameof(instance));
            _context = Check.NotNull(context, nameof(context));
            _logger = Check.NotNull(context.Logger, nameof(context.Logger));
        }

        /// <summary>
        /// Отвечает за логирование действий при обращении к методам инстанса. По умолчанию берется 
        /// значение из конфигурации контекста <see cref="BotContextConfiguration.IsTraceLoggingEnabled"/>.
        /// </summary>
        public OptionState IsLoggingEnabled { get; set; }

        /// <summary>
        /// Возвращает инстанс браузера <see cref="ZennoLab.CommandCenter.Instance"/>.
        /// </summary>
        public ZennoLab.CommandCenter.Instance ZennoInstance { get; }

        /// <summary>
        /// Возвращает активную вкладку.
        /// </summary>
        public Tab ActiveTab => ZennoInstance.ActiveTab.ToExtended(_context);

        /// <summary>
        /// Возвращает главную вкладку. Вкладка с именем "page" или последняя созданная.
        /// </summary>
        public Tab MainTab => ZennoInstance.MainTab.ToExtended(_context);

        /// <summary>
        /// Задает прокси для инстанса <see cref="ZennoLab.CommandCenter.Instance"/> браузера.
        /// </summary>
        /// <param name="ip">IP адрес прокси.</param>
        /// <param name="port">Порт.</param>
        /// <param name="type">Тип прокси. Может быть http, socks4 или socks5.</param>
        /// <param name="login">Логин для авторизации.</param>
        /// <param name="password">Пароль для авторизации.</param>
        /// <param name="useProxifier"><c>true</c>, если нужно использовать proxifier; в противном случае <c>false</c>.
        /// <para>Аргумент доступен с версии ZennoPoster <c>5.9.7</c>.</para></param>
        /// <param name="emulateGeolocation"><c>true</c>, если нужно эмулировать геолокацию в соответствии с ip; в противном случае <c>false</c>.
        /// <para>Аргумент доступен с версии ZennoPoster <c>5.10.5.0</c>.</para></param>
        /// <param name="emulateTimezone"><c>true</c>, если нужно эмулировать часовой пояс в соответствии с ip; в противном случае <c>false</c>.
        /// <para>Аргумент доступен с версии ZennoPoster <c>5.10.5.0</c>.</para></param>
        /// <returns></returns>
        /// <remarks>Использование аргумента <c>useProxifier</c> доступно с версии ZennoPoster <c>5.9.7</c>.</remarks>
        /// <remarks>Использование аргументов <c>emulateGeolocation</c>, <c>emulateTimezone</c> доступно с версии ZennoPoster <c>5.10.5.0</c>.</remarks>
        public Instance SetProxy(string ip, int port, string type, string login = null, string password = null,
            bool useProxifier = false, bool emulateGeolocation = false, bool emulateTimezone = false)
        {
            // TODO: добавить вызов методов через dynamic для совместимости
            InvokeLogging($"SetProxy(\"{ip}\", {port}, \"{type}\", \"{login}\", \"{password}\", " +
                     $"{useProxifier}, {emulateGeolocation}, {emulateTimezone})");
            ZennoInstance.SetProxy(ip, port, type, login, password, useProxifier);
            return this;
        }

        /// <summary>
        /// Задает прокси для инстанса <see cref="ZennoLab.CommandCenter.Instance"/> браузера.
        /// </summary>
        /// <param name="proxyString">Параметры прокси. Эта строка должна иметь следующий формат:
        /// [protocol://][(login):(password)@](ip):(port). Например: socks5://192.168.0.1:80</param>
        /// <param name="useProxifier"><c>true</c>, если нужно использовать proxifier; в противном случае <c>false</c>.
        /// <para>Аргумент доступен с версии ZennoPoster <c>5.9.7</c>.</para></param>
        /// <param name="emulateGeolocation"><c>true</c>, если нужно эмулировать геолокацию в соответствии с ip; в противном случае <c>false</c>.
        /// <para>Аргумент доступен с версии ZennoPoster <c>5.10.5.0</c>.</para></param>
        /// <param name="emulateTimezone"><c>true</c>, если нужно эмулировать часовой пояс в соответствии с ip; в противном случае <c>false</c>.
        /// <para>Аргумент доступен с версии ZennoPoster <c>5.10.5.0</c>.</para></param>
        /// <returns></returns>
        /// <remarks>Использование аргумента <c>useProxifier</c> доступно с версии ZennoPoster <c>5.9.7</c>.</remarks>
        /// <remarks>Использование аргументов <c>emulateGeolocation</c>, <c>emulateTimezone</c> доступно с версии ZennoPoster <c>5.10.5.0</c>.</remarks>
        public Instance SetProxy(string proxyString, bool useProxifier = false, bool emulateGeolocation = false,
            bool emulateTimezone = false)
        {
            // TODO: добавить вызов методов через dynamic для совместимости
            InvokeLogging($"SetProxy(\"{proxyString}\", {useProxifier}, {emulateGeolocation}, {emulateTimezone})");
            ZennoInstance.SetProxy(proxyString, useProxifier, emulateGeolocation, emulateTimezone);
            return this;
        }

        /// <summary>
        /// Закрывает все вкладки, кроме главной.
        /// </summary>
        /// <returns></returns>
        public Instance CloseAllTabs()
        {
            InvokeLogging("CloseAllTabs()");
            ZennoInstance.CloseAllTabs();
            return this;
        }

        /// <summary>
        /// Очищает кэш.
        /// </summary>
        /// <returns></returns>
        public Instance ClearCache()
        {
            InvokeLogging("ClearCache()");
            ZennoInstance.ClearCache();
            return this;
        }

        /// <summary>
        /// Очищает куки.
        /// </summary>
        /// <returns></returns>
        public Instance ClearCookie()
        {
            InvokeLogging("ClearCookie()");
            ZennoInstance.ClearCookie();
            return this;
        }

        /// <summary>
        /// Удаляет прокси из инстанса <see cref="ZennoLab.CommandCenter.Instance"/> браузера.
        /// </summary>
        /// <returns></returns>
        public Instance ClearProxy()
        {
            InvokeLogging("ClearProxy()");
            ZennoInstance.ClearProxy();
            return this;
        }

        /// <summary>
        /// Перезапускает текущий инстанс браузера. Работает только в ProjectMaker и ZennoPoster.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Доступно с версии ZennoPoster <c>5.10.4.0</c>.</remarks>
        public Instance Reload()
        {
            InvokeLogging("Reload()");
            ZennoInstance.Reload();
            return this;
        }

        /// <summary>
        /// Эмулирует ввод с клавиатуры.
        /// </summary>
        /// <param name="text">Текст для ввода.</param>
        /// <param name="latency">Задержка между вводимыми символами текста. 
        /// По умолчанию генерируется в соответствии с параметрами <see cref="BotContextConfiguration.KeyboardEmulationLatency"/>.</param>
        /// <returns></returns>
        public Instance SendText(string text, int? latency = null)
        {
            var currentLatency = latency ?? _context.Configuration.KeyboardEmulationLatency.Next();
            InvokeLogging($"SendText(\"{text}\", {currentLatency})");
            ZennoInstance.SendText(text, currentLatency);
            return this;
        }

        /// <summary>
        /// Задает политику для окон отправки файла на сервер.
        /// </summary>
        /// <param name="answer">Ответ для окна авторизации. Этот параметр может принимать значения: "ok" или "cancel".</param>
        /// <param name="value">Значение ответа.</param>
        /// <returns></returns>
        public Instance SetFileUploadPolicy(string answer, string value)
        {
            // TODO: добавить 3-й параметр id
            InvokeLogging($"SetFileUploadPolicy(\"{answer}\", \"{value}\")");
            ZennoInstance.SetFileUploadPolicy(answer, value);
            return this;
        }

        /// <summary>
        /// Устанавливает файлы для загрузки.
        /// </summary>
        /// <param name="files">Пути файлов для загрузки. Разделяются через ",".</param>
        /// <returns></returns>
        /// <remarks>Доступно с версии ZennoPoster <c>5.11.6.0</c>.</remarks>
        public Instance SetFilesForUpload(string files)
        {
            InvokeLogging($"SetFilesForUpload(\"{files}\")");
            ZennoInstance.SetFilesForUpload(files);
            return this;
        }

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }

        private void InvokeLogging(string message)
        {
            if (IsLoggingEnabled == OptionState.Default && _context.Configuration.IsTraceLoggingEnabled ||
                IsLoggingEnabled == OptionState.Enabled)
                _logger.Trace($"Instance.{message}");
        }
    }
}
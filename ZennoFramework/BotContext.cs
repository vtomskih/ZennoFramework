using System.Collections.Generic;
using ZennoFramework.Extensions;
using ZennoFramework.Interception;
using ZennoFramework.Logging;
using ZennoFramework.Logging.Abstractions;
using ZennoFramework.Utilities;
using ZennoLab.InterfacesLibrary.ProjectModel;
using Instance = ZennoFramework.Infrastructure.Instance;

namespace ZennoFramework
{
    /// <summary>
    /// Представляет контекст проекта.
    /// </summary>
    public abstract class BotContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BotContext"/>.
        /// </summary>
        /// <param name="instance">Экземпляр инстанса браузера <see cref="ZennoLab.CommandCenter.Instance"/>.</param>
        /// <param name="project">Экземпляр модели проекта <see cref="IZennoPosterProjectModel"/>.</param>
        /// <param name="loggerFactory">Фабрика логеров. Необязательный параметр.</param>
        public BotContext(ZennoLab.CommandCenter.Instance instance, IZennoPosterProjectModel project,
            ILoggerFactory loggerFactory = null)
        {
            Check.NotNull(instance, nameof(instance));
            Project = Check.NotNull(project, nameof(project));

            Configuration = new BotContextConfiguration();
            Interception = new Interception.Interception();

            loggerFactory = loggerFactory ?? new LoggerFactory();
            Configure(loggerFactory);
            Logger = loggerFactory.CreateLogger(GetType().Name);

            Instance = instance.ToExtended(this);
        }

        internal Dictionary<string, string> KeysAndXpaths;

        /// <summary>
        /// Содержит глобальные настройки контекста.
        /// </summary>
        public BotContextConfiguration Configuration { get; }

        /// <summary>
        /// Представляет инстанс браузера. Содержит необходимые методы и свойства для работы с инстансом.
        /// </summary>
        public Instance Instance { get; }

        /// <summary>
        /// Представляет модель текущего проекта.
        /// Предоставляет доступ к глобальным и локальным переменным, таблицам, профилям, спискам.
        /// Содержит методы отправки сообщений в лог программы.
        /// </summary>
        public IZennoPosterProjectModel Project { get; }

        /// <summary>
        /// Логер. Любую отправку сообщений в лог рекомендуется выполнять через данное свойство.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Представляет механизм перехвата действий с html элементами.
        /// </summary>
        public Interception.Interception Interception { get; }

        /// <summary>
        /// Предоставляет возможность выполнить настройку логирования.
        /// </summary>
        /// <param name="loggerFactory">Экземпляр фабрики логеров.</param>
        protected virtual void Configure(ILoggerFactory loggerFactory)
        {
        }
    }
}
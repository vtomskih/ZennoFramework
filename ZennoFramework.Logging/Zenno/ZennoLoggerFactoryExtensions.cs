using System;
using ZennoFramework.Logging.Abstractions;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZennoFramework.Logging.Zenno
{
    public static class ZennoLoggerFactoryExtensions
    {
        public static ILoggerFactory AddZenno(this ILoggerFactory factory, IZennoPosterProjectModel project,
            Func<string, LogLevel, bool, bool> filter, Func<object, Exception, string> formatter = null)
        {
            factory.AddProvider(new ZennoLoggerProvider(project, filter, formatter));
            return factory;
        }

        public static ILoggerFactory AddZenno(this ILoggerFactory factory, IZennoPosterProjectModel project,
            LogLevel minLevel = LogLevel.Trace, LogLevel zennoPosterMinLevel = LogLevel.Info,
            Func<object, Exception, string> formatter = null)
        {
            return AddZenno(factory, project,
                (_, logLevel, isPoster) => isPoster ? logLevel >= zennoPosterMinLevel : logLevel >= minLevel,
                formatter);
        }
    }
}
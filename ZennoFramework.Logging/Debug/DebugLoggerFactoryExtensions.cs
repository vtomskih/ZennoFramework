using System;
using ZennoFramework.Logging.Abstractions;

namespace ZennoFramework.Logging.Debug
{
    public static class DebugLoggerFactoryExtensions
    {
        public static ILoggerFactory AddDebug(this ILoggerFactory factory, Func<string, LogLevel, bool> filter)
        {
            factory.AddProvider(new DebugLoggerProvider(filter));
            return factory;
        }

        public static ILoggerFactory AddDebug(this ILoggerFactory factory, LogLevel minLevel = LogLevel.Info)
        {
            return AddDebug(
                factory,
                (_, logLevel) => logLevel >= minLevel);
        }
    }
}
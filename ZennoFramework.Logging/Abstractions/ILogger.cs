using System;

namespace ZennoFramework.Logging.Abstractions
{
    public interface ILogger
    {
        bool IsEnabled(LogLevel logLevel);
        void Log<TState>(LogLevel logLevel, TState state, Exception exception, Func<TState, Exception, string> formatter);
    }
}
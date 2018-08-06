using System;

namespace ZennoFramework.Logging.Abstractions
{
    public interface ILoggerProvider : IDisposable
    {
        ILogger CreateLogger(string categoryName);
    }
}
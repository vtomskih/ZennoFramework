using System;
using ZennoFramework.Logging.Abstractions;

namespace ZennoFramework.Logging.Debug
{
    public class DebugLoggerProvider: ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;

        public DebugLoggerProvider()
        {
            _filter = null;
        }

        public DebugLoggerProvider(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
        }

        public ILogger CreateLogger(string name)
        {
            return new DebugLogger(name, _filter);
        }

        public void Dispose()
        {   
        }
    }
}
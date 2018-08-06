using System;
using ZennoFramework.Logging.Abstractions;

namespace ZennoFramework.Logging
{
    public struct LoggerInfo
    {
        public ILogger Logger { get; set; }
        public string Category { get; set; }
        public Type ProviderType { get; set; }
        public LogLevel? MinLevel { get; set; }
        public Func<string, string, LogLevel, bool> Filter { get; set; }

        public bool IsEnabled(LogLevel level)
        {
            if (MinLevel != null && level < MinLevel)
            {
                return false;
            }

            if (Filter != null)
            {
                return Filter(ProviderType.FullName, Category, level);
            }

            return true;
        }
    }
}
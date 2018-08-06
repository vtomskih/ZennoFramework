using System;
using System.Diagnostics;
using ZennoFramework.Logging.Abstractions;

namespace ZennoFramework.Logging.Debug
{
    public class DebugLogger : ILogger
    {
        private readonly string _name;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly Func<object, Exception, string> _formatter;

        public DebugLogger(string name, Func<string, LogLevel, bool> filter = null,
            Func<object, Exception, string> formatter = null)
        {
            _name = string.IsNullOrEmpty(name) ? nameof(DebugLogger) : name;
            _filter = filter;
            _formatter = formatter;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Debugger.IsAttached && logLevel != LogLevel.None && (_filter == null || _filter(_name, logLevel));
        }

        public void Log<TState>(LogLevel logLevel, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            formatter = formatter ?? MessageFormatter;
            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"{logLevel}: {message}";

            if (exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception;
            }

            WriteLine(message, _name);
        }

        private void WriteLine(string message, string name)
        {
            System.Diagnostics.Debug.WriteLine(message, name);
        }

        private string MessageFormatter<TState>(TState state, Exception exception)
        {
            if (_formatter != null)
            {
                return _formatter(state, exception);
            }

            var message = state?.ToString();

            if (exception != null)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += Environment.NewLine;
                }

                message += exception;
            }

            return message;
        }
    }
}
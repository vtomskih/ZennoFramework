using System;
using ZennoFramework.Logging.Abstractions;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZennoFramework.Logging.Zenno
{
    public class ZennoLogger : ILogger
    {
        private readonly IZennoPosterProjectModel _project;
        private readonly string _name;
        private readonly Func<string, LogLevel, bool, bool> _filter;
        private readonly Func<object, Exception, string> _formatter;

        public ZennoLogger(IZennoPosterProjectModel project, string name, Func<string, LogLevel, bool, bool> filter = null,
            Func<object, Exception, string> formatter = null)
        {
            _name = string.IsNullOrEmpty(name) ? nameof(ZennoLogger) : name;
            _project = project;
            _filter = filter;
            _formatter = formatter;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None && (_filter == null || _filter(_name, logLevel, false));
        }

        public void Log<TState>(LogLevel logLevel, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            formatter = formatter ?? MessageFormatter;
            var message = formatter(state, exception);
            WriteMessage(logLevel, message);
        }

        private void WriteMessage(LogLevel logLevel, string message)
        {
            var showInPoster = _filter?.Invoke(_name, logLevel, true) ?? false;

            switch (logLevel)
            {
                case LogLevel.Trace:
                    _project.SendInfoToLog($"Trace: {message}", showInPoster);
                    break;
                case LogLevel.Debug:
                    _project.SendInfoToLog($"Debug: {message}", showInPoster);
                    break;
                case LogLevel.Info:
                    _project.SendInfoToLog(message, showInPoster);
                    break;
                case LogLevel.Warning:
                    _project.SendWarningToLog(message, showInPoster);
                    break;
                case LogLevel.Error:
                    _project.SendErrorToLog(message, showInPoster);
                    break;
                case LogLevel.Critical:
                    _project.SendErrorToLog(message, showInPoster);
                    break;
            }
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
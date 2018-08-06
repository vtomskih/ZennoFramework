using System;
using System.Collections.Generic;
using ZennoFramework.Logging.Abstractions;

namespace ZennoFramework.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        private volatile bool _disposed;
        private readonly Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>(StringComparer.Ordinal);
        private readonly List<ProviderRegistration> _providerRegistrations = new List<ProviderRegistration>();
        private readonly object _sync = new object();

        public LoggerFactory(IEnumerable<ILoggerProvider> providers = null)
        {
            if (providers == null)
            {
                return;
            }

            foreach (var provider in providers)
            {
                AddProviderRegistration(provider, false);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(LoggerFactory));
            }

            lock (_sync)
            {
                if (!_loggers.TryGetValue(categoryName, out var logger))
                {
                    logger = new Logger(this) {Loggers = CreateLoggers(categoryName)};
                    _loggers[categoryName] = logger;
                }

                return logger;
            }
        }

        public void AddProvider(ILoggerProvider provider)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(LoggerFactory));
            }

            AddProviderRegistration(provider, true);

            lock (_sync)
            {
                foreach (var logger in _loggers)
                {
                    var loggerInformation = logger.Value.Loggers;
                    var categoryName = logger.Key;

                    Array.Resize(ref loggerInformation, loggerInformation.Length + 1);
                    var newLoggerIndex = loggerInformation.Length - 1;

                    SetLoggerInfo(ref loggerInformation[newLoggerIndex], provider, categoryName);

                    logger.Value.Loggers = loggerInformation;
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            foreach (var registration in _providerRegistrations)
            {
                try
                {
                    if (registration.ShouldDispose)
                    {
                        registration.Provider.Dispose();
                    }
                }
                catch
                {
                    // Swallow exceptions on dispose
                }
            }
        }

        private void AddProviderRegistration(ILoggerProvider provider, bool dispose)
        {
            _providerRegistrations.Add(new ProviderRegistration {Provider = provider, ShouldDispose = dispose});
        }

        private void SetLoggerInfo(ref LoggerInfo loggerInfo, ILoggerProvider provider, string categoryName)
        {
            loggerInfo.Logger = provider.CreateLogger(categoryName);
            loggerInfo.ProviderType = provider.GetType();
        }

        private LoggerInfo[] CreateLoggers(string categoryName)
        {
            var loggers = new LoggerInfo[_providerRegistrations.Count];
            for (int i = 0; i < _providerRegistrations.Count; i++)
            {
                SetLoggerInfo(ref loggers[i], _providerRegistrations[i].Provider, categoryName);
            }

            return loggers;
        }

        private struct ProviderRegistration
        {
            public ILoggerProvider Provider;
            public bool ShouldDispose;
        }
    }
}
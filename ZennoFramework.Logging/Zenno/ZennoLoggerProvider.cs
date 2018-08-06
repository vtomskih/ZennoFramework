using System;
using ZennoFramework.Logging.Abstractions;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZennoFramework.Logging.Zenno
{
    public class ZennoLoggerProvider : ILoggerProvider
    {
        private readonly IZennoPosterProjectModel _zennoProject;
        private readonly Func<string, LogLevel, bool, bool> _filter;
        private readonly Func<object, Exception, string> _formatter;

        public ZennoLoggerProvider(IZennoPosterProjectModel zennoProject, Func<string, LogLevel, bool, bool> filter,
            Func<object, Exception, string> formatter = null)
        {
            _zennoProject = zennoProject;
            _filter = filter;
            _formatter = formatter;
        }

        public ILogger CreateLogger(string name)
        {
            return new ZennoLogger(_zennoProject, name, _filter, _formatter);
        }

        public void Dispose()
        {
        }
    }
}
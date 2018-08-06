using ZennoFramework;
using ZennoFramework.Logging.Abstractions;
using ZennoFramework.Logging.Zenno;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace TestBot
{
    public class TestBot : BotContext
    {
        public TestBot(ZennoLab.CommandCenter.Instance instance, IZennoPosterProjectModel project) : base(instance, project)
        {
        }

        protected override void Configure(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddZenno(Project);
        }
    }
}   
using System;
//using Test;
using ZennoFramework.Interception;
using ZennoFramework.Logging.Abstractions;
using ZennoFramework.Logging.Zenno;
using ZennoFramework.Tests.Pages.Login;
using ZennoFramework.Tests.Pages.Login.Elements;
using ZennoFramework.Xml;
using ZennoLab.InterfacesLibrary.ProjectModel;
using LogLevel = ZennoFramework.Logging.LogLevel;

namespace ZennoFramework.Tests
{
    public class TestContext : BotContext
    {
        public TestContext(ZennoLab.CommandCenter.Instance instance, IZennoPosterProjectModel project) : base(instance, project)
        {
            Configuration.IsAutoWaitDownloadingEnabled = true;
            LoginPage = new LoginPage(this);

            var keysAndXpaths = XmlParser.GetKeysAndXPaths(@"D:\Dev\ZennoFramework\XmlParser\SiteData\Elements.xml");
            //Elements = new Elements<Element>(this, keysAndXpaths);
        }

        public LoginPage LoginPage { get; }
        //public Elements<Element> Elements { get; set; }

        protected override void Configure(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddZenno(Project, LogLevel.Trace);
        }
    }
}   
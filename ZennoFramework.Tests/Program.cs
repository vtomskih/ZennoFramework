using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZennoFramework.Tests
{
    public class Program : IZennoCustomCode
    {
        public int ExecuteCode(ZennoLab.CommandCenter.Instance instance, IZennoPosterProjectModel project)
        {
            var ctx = new TestContext(instance, project);
            //ctx.Elements.LoginPage.LoginBtn.ScrollIntoView().WithoutLogging().GetValue();
            return 1;
        }
    }
}
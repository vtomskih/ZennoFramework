using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace TestBot
{
    public class Program : IZennoCustomCode
    {
        public int ExecuteCode(Instance instance, IZennoPosterProjectModel project)
        {
            var bot = new TestBot(instance, project);
            bot.Instance.ActiveTab.Navigate("ya.ru");

            return 0;
        }
    }
}

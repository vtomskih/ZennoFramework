using ZennoFramework.Infrastructure.Elements;

namespace ZennoFramework.Tests.Pages.Login.Elements
{
    public class LoginButton : Element
    {
        public LoginButton(BotContext context) : base(context)
        {
        }

        protected override Element Find()
        {
            return Context.Instance.ActiveTab.FindElementByXPath("//button[@login]");
        }
    }
}
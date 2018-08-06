using ZennoFramework.Infrastructure.Elements;

namespace ZennoFramework.Tests.Pages.Login.Elements
{
    public class LoginField : Element
    {
        public LoginField(BotContext context) : base(context)
        {
        }

        public void SetLogin()
        {
            SetValue("mylogin");
        }

        protected override Element Find()
        {
            return Context.Instance.ActiveTab.FindElementByXPath("//button[@login]");
        }
    }
}
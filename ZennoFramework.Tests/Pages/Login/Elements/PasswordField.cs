using ZennoFramework.Infrastructure.Elements;

namespace ZennoFramework.Tests.Pages.Login.Elements
{
    public class PasswordField : Element
    {
        public PasswordField(BotContext context) : base(context)
        {
        }

        public void SetPassword()
        {
            Click();
            SetValue("password");
        }

        protected override Element Find()
        {
            return Context.Instance.ActiveTab.FindElementByXPath("//button[@login]");
        }
    }
}
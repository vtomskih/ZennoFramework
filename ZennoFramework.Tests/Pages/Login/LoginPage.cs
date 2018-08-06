using ZennoFramework.Infrastructure;
using ZennoFramework.Infrastructure.Elements;
using ZennoFramework.Tests.Pages.Login.Elements;

namespace ZennoFramework.Tests.Pages.Login
{
    public class LoginPage : ElementContainer<BotContext>
    {
        private LoginButtonWrap Button2 { get; }
        private LoginField LoginField { get; }

        private LoginButton LoginButton { get; }
        private PasswordField PasswordField { get; }

        public LoginPage(BotContext context) : base(context)
        {
            Button2 = new LoginButtonWrap(context);
            Links = new Links(context);
            LoginField = new LoginField(context);
            PasswordField = new PasswordField(context);
            LoginButton = new LoginButton(context);
        }

        public Links Links { get; }

        public void Login()
        {
            LoginField.SetLogin();
            PasswordField.SetPassword();
            LoginButton.Click();
        }
    }
}
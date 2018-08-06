using ZennoFramework.Infrastructure;
using ZennoFramework.Infrastructure.Elements;

namespace ZennoFramework.Tests.Pages.Login.Elements
{
    public class Links : ElementCollection<LoginButton>
    {
        public Links(BotContext context) : base(context)
        {
        }
        
        /// <inheritdoc />
        protected override ElementCollection Find()
        {
            return Context.Instance.ActiveTab.FindElementsByXPath("//a");
        }
    }
}
using Tab = ZennoFramework.Infrastructure.Tab;

namespace ZennoFramework.Extensions
{
    public static class ZennoTabExtensions
    {
        public static Tab ToExtended(this ZennoLab.CommandCenter.Tab tab, BotContext context)
        {
            return new Tab(tab, context);
        }
    }
}
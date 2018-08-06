using Instance = ZennoFramework.Infrastructure.Instance;

namespace ZennoFramework.Extensions
{
    public static class ZennoInstanceExtensions
    {
        public static Instance ToExtended(this ZennoLab.CommandCenter.Instance instance, BotContext context)
        {
            return new Instance(instance, context);
        }
    }
}
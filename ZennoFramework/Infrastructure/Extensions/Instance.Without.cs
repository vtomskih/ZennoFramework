using ZennoFramework.Enums;

namespace ZennoFramework.Infrastructure.Extensions
{
    public static class InstanceExtensions
    {
        /// <summary>
        /// Возвращает копию текущего инстанса браузера, у которой отключено логирование действий
        /// (<see cref="Instance.IsLoggingEnabled"/> = false).
        /// </summary>
        public static Instance WithoutLogging(this Instance @this)
        {
            var instance = @this.IsLoggingEnabled == OptionState.Disabled ? @this : (Instance)@this.Clone();
            instance.IsLoggingEnabled = OptionState.Disabled;
            return instance;
        }
    }
}
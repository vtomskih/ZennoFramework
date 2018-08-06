using ZennoFramework.Enums;

namespace ZennoFramework.Infrastructure.Extensions
{
    public static class TabExtensions
    {
        /// <summary>
        /// Возвращает копию текущей вкладки, у которой отключено логирование действий
        /// (<see cref="Tab.IsLoggingEnabled"/> = false).
        /// </summary>
        public static Tab WithoutLogging(this Tab @this)
        {
            var tab = @this.IsLoggingEnabled == OptionState.Disabled ? @this : (Tab)@this.Clone();
            tab.IsLoggingEnabled = OptionState.Disabled;
            return tab;
        }

        /// <summary>
        /// Возвращает копию текущей вкладки, у которой отключено автоматическое ожидание загрузки страницы
        /// (<see cref="Tab.IsAutoWaitDownloadingEnabled"/> = false).
        /// </summary>
        /// <param name="this"></param>
        public static Tab WithoutWaitDownloading(this Tab @this)
        {
            var tab = @this.IsAutoWaitDownloadingEnabled == OptionState.Disabled ? @this : (Tab)@this.Clone();
            tab.IsAutoWaitDownloadingEnabled = OptionState.Disabled;
            return tab;
        }
    }
}
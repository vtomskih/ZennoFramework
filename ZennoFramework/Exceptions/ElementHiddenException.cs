using ZennoFramework.Infrastructure.Elements;

namespace ZennoFramework.Exceptions
{
    /// <summary>
    /// Исключение, которое выбрасывается, если элемент скрыт - не отображается на странице (<see cref="Element.IsHidden"/> == <c>true</c>).
    /// </summary>
    public class ElementHiddenException : ZennoFrameworkException
    {
        /// <inheritdoc />
        public ElementHiddenException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public ElementHiddenException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
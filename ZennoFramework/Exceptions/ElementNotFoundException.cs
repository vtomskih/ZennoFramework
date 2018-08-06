using ZennoFramework.Infrastructure.Elements;

namespace ZennoFramework.Exceptions
{
    /// <summary>
    /// Исключение, которое выбрасывается, если элемент не найден на странице (<see cref="Element.IsNull"/> == <c>true</c>).
    /// </summary>
    public class ElementNotFoundException : ZennoFrameworkException
    {
        /// <inheritdoc />
        public ElementNotFoundException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public ElementNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
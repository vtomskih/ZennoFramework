namespace ZennoFramework.Exceptions
{
    /// <summary>
    /// Базовый класс для исключений, выбрасываемых библиотекой.
    /// </summary>
    public class ZennoFrameworkException : System.Exception
    {
        /// <inheritdoc />
        public ZennoFrameworkException()
        {
        }

        /// <inheritdoc />
        public ZennoFrameworkException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public ZennoFrameworkException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
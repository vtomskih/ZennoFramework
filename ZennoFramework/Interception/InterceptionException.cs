using System;
using ZennoFramework.Exceptions;

namespace ZennoFramework.Interception
{
    public class InterceptionException : ZennoFrameworkException
    {
        public InterceptionException(string message) : base(message)
        {
        }

        public InterceptionException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
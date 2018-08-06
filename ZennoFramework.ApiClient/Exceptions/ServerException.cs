using System;

namespace ZennoFramework.ApiClient.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException(string message) : base(message)
        {
            
        }
    }
}

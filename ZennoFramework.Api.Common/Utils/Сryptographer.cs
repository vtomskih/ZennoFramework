using ZennoFramework.Api.Common.Messages;

namespace ZennoFramework.Api.Common.Utils
{
    public static class Cryptographer
    {
        public static string Encrypt<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);

            return StringUtils.Encrypt(json);
        }
            
        public static T Decrypt<T>(string message)
        {
            var json = StringUtils.Decrypt(message);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}   
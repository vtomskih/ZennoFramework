using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ZennoFramework.Api.Common.Utils
{
    public class JsonSerializer
    {
        public static string Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(obj.GetType());
                ser.WriteObject(stream, obj);

                stream.Position = 0;
                using (var sr = new StreamReader(stream))   
                {
                    return sr.ReadToEnd();
                }   
            }
        }

        public static T Deserialize<T>(string objectData)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(objectData)))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(stream);
            }
        }
    }
}

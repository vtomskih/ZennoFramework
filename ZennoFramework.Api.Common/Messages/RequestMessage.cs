using System.Runtime.Serialization;

namespace ZennoFramework.Api.Common.Messages
{
    [DataContract]
    public class RequestMessage
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Hardware { get; set; }

        [DataMember]
        public GenerationData GenerationData { get; set; }
    }           
}
    
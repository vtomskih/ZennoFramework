using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZennoFramework.Api.Common.Messages
{
    [DataContract]
    public class GenerationData
    {
        [DataMember]
        public string Namespace { get; set; }

        [DataMember]
        public IEnumerable<string> XmlDocuments { get; set; }
    }
}
using System.Runtime.Serialization;

namespace ZennoFramework.Api.Common.Messages
{
    [DataContract]  
    public class ResponseMessage
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "serverError")]
        public string ServerError { get; set; }

        [DataMember(Name = "generatorError")]
        public string GeneratorError { get; set; }
    }
}

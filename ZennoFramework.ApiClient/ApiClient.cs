using ZennoFramework.Api.Client.Utils;
using ZennoFramework.ApiClient.Exceptions;
using ZennoFramework.Api.Common.Utils;
using ZennoFramework.Api.Common.Messages;

namespace ZennoFramework.Api.Client
{
    public class ApiClient
    {   
        private string _apiUrl;

        public ApiClient()
        {
            _apiUrl = "http://4442.ru/api/license";
        }

        public ApiClient(string url)
        {
            _apiUrl = url;
        }

        public string GenerateCode(string licenseKey, GenerationData generationData)
        {
            var hw = Hardware.GetId();
            var body = GenerateRequestBody(licenseKey, hw, generationData);
            var task = RequestHelper.PostAsync(_apiUrl, body);
            task.Wait();
           
            return GetResultFromResponse(task.Result);
        }

        private string GenerateRequestBody(string licenseKey, string hw, GenerationData generationData)
        {
            var message = new RequestMessage
            {
                Key = licenseKey,
                Hardware = hw,  
                GenerationData = generationData
            };

            return Cryptographer.Encrypt(message);
        }
        
        private string GetResultFromResponse(string responseBody)
        {
            var message = JsonSerializer.Deserialize<ResponseMessage>(responseBody);
            ThrowServerException(message);
            ThrowGeneratorException(message);

            return message.Code;
        }

        private static void ThrowServerException(ResponseMessage message)
        {
            if (!string.IsNullOrWhiteSpace(message.ServerError))
            {
                throw new CodeGeneratorException(message.ServerError);
            }
        }

        private static void ThrowGeneratorException(ResponseMessage message)
        {
            if (!string.IsNullOrWhiteSpace(message.GeneratorError))
            {
                throw new CodeGeneratorException(message.GeneratorError);
            }
        }

    }   
}

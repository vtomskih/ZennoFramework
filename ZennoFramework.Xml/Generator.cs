using System.Linq;
using ZennoFramework.Api.Common.Messages;
using ZennoFramework.Xml.Extensions;
using ZennoFramework.Xml.Validation;

namespace ZennoFramework.Xml
{
    public class Generator
    {
        private readonly string _licenseKey;
        private readonly Api.Client.ApiClient _apiClient;

        public Generator(string licenseKey)
        {
            _licenseKey = licenseKey;
            _apiClient = new Api.Client.ApiClient();
        }

        public string GenerateElements(string path, string elementsNamespace = "")
        {
            var docs = Loader.Load(path);
            Validator.CheckDocuments(docs);
            LocatorRenderer.Run(docs);
            var docsString = docs.Values.Select(x => x.Save());
            var data = new GenerationData{Namespace = elementsNamespace, XmlDocuments = docsString};
            return _apiClient.GenerateCode(_licenseKey, data);
        }
    }
}
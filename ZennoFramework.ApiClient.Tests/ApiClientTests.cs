using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ZennoFramework.Api.Common.Messages;
using ZennoFramework.Api.Common.Utils;

namespace ZennoFramework.ApiClient.Tests
{
    [TestFixture]
    public class ApiClientTests
    {
        private Api.Client.ApiClient _client = new Api.Client.ApiClient();

        [Test]
        public void Test()
        {
            var key = "ta";
            var data = new GenerationData{Namespace = "test"};
            var code = _client.GenerateCode(key, data);
            Assert.IsNull(code);
        }

        [Test]
        public void SerializeDeserializeTest()
        {
            var obj = new ResponseMessage
            {
                ServerError = "Check"
            };

            var json = JsonSerializer.Serialize(obj);
            var obj2 = JsonSerializer.Deserialize<ResponseMessage>(json);

            Assert.AreEqual(obj.ServerError, obj2.ServerError);
        }
    }
}

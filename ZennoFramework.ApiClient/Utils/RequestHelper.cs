using System.Net.Http;
using System.Threading.Tasks;

namespace ZennoFramework.Api.Client.Utils
{
    public static class RequestHelper
    {
        public static async Task<string> PostAsync(string url, string body)
        {   
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, new StringContent(body)).ConfigureAwait(false);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}

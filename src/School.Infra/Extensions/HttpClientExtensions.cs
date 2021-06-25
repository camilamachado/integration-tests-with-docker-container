using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace School.Infra.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string url, object content)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            };

            return client.SendAsync(request);
        }

        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, object content)
        {
            return client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
        }

        public static Task<HttpResponseMessage> PutAsync(this HttpClient client, string url, object content)
        {
            return client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string url, object content)
        {
            return client.PatchAsync(url, new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
        }
    }
}

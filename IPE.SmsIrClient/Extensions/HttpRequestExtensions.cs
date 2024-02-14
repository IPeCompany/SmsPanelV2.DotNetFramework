using IPE.SmsIrClient.Handlers;
using IPE.SmsIrClient.Models.Results;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPE.SmsIrClient.Extensions
{
    internal static class HttpRequestExtensions
    {
        internal static async Task<SmsIrResult<TResult>> GetRequestAsync<TResult>(this HttpClient httpClient, string requestUri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

            return await HandleResponseAsync<TResult>(response);
        }

        internal static SmsIrResult<TResult> GetRequest<TResult>(this HttpClient httpClient, string requestUri)
        {
            HttpResponseMessage response = httpClient.GetAsync(requestUri).GetAwaiter().GetResult();

            return HandleResponse<TResult>(response);
        }

        internal static async Task<SmsIrResult<TResult>> PostRequestAsync<TResult>(this HttpClient httpClient, string requestUri, object data)
        {
            string payload = JsonConvert.SerializeObject(data);

            StringContent httpContent = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(requestUri, httpContent);

            return await HandleResponseAsync<TResult>(response);
        }

        internal static SmsIrResult<TResult> PostRequest<TResult>(this HttpClient httpClient, string requestUri, object data)
        {
            string payload = JsonConvert.SerializeObject(data);

            StringContent httpContent = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.PostAsync(requestUri, httpContent).GetAwaiter().GetResult();

            return HandleResponse<TResult>(response);
        }

        internal static async Task<SmsIrResult<TResult>> DeleteRequestAsync<TResult>(this HttpClient httpClient, string requestUri)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(requestUri);

            return await HandleResponseAsync<TResult>(response);
        }

        internal static SmsIrResult<TResult> DeleteRequest<TResult>(this HttpClient httpClient, string requestUri)
        {
            HttpResponseMessage response = httpClient.DeleteAsync(requestUri).GetAwaiter().GetResult();

            return HandleResponse<TResult>(response);
        }

        private static async Task<SmsIrResult<TResult>> HandleResponseAsync<TResult>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                await HttpResponseHandler.HandleUnsuccessfulResponseAsync(response);
            }

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SmsIrResult<TResult>>(content);
        }

        private static SmsIrResult<TResult> HandleResponse<TResult>(HttpResponseMessage response)
        {
            return HandleResponseAsync<TResult>(response).GetAwaiter().GetResult();
        }
    }
}
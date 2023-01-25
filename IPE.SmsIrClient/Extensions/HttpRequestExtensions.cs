using IPE.SmsIrClient.Exceptions;
using IPE.SmsIrClient.Models.Results;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPE.SmsIrClient.Extensions
{
    internal static class HttpRequestExtensions
    {
        internal static async Task<SmsIrResult<TResult>> GetRequestAsync<TResult>(this HttpClient httpClient, string requestUri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                await HandleUnsuccessfulResponse(response);

            return JsonConvert.DeserializeObject<SmsIrResult<TResult>>(response.Content.ReadAsStringAsync().Result);
        }

        internal static async Task<SmsIrResult<TResult>> PostRequestAsync<TResult>(this HttpClient httpClient, string requestUri, object data)
        {
            string payload = JsonConvert.SerializeObject(data);

            StringContent httpContent = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(requestUri, httpContent);

            if (!response.IsSuccessStatusCode)
                await HandleUnsuccessfulResponse(response);

            return JsonConvert.DeserializeObject<SmsIrResult<TResult>>(response.Content.ReadAsStringAsync().Result);
        }

        internal static async Task<SmsIrResult<TResult>> DeleteRequestAsync<TResult>(this HttpClient httpClient, string requestUri)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                await HandleUnsuccessfulResponse(response);

            return JsonConvert.DeserializeObject<SmsIrResult<TResult>>(response.Content.ReadAsStringAsync().Result);
        }

        private static Task HandleUnsuccessfulResponse(HttpResponseMessage response)
        {
            SmsIrResult baseResponse = JsonConvert.DeserializeObject<SmsIrResult>(response.Content.ReadAsStringAsync().Result);
            int statusCode = (int)response.StatusCode;

            switch (statusCode)
            {
                case 401:
                    throw new UnauthorizedException(baseResponse.Status, baseResponse.Message);
                case 400:
                    throw new LogicalException(baseResponse.Status, baseResponse.Message);
                case 429:
                    throw new TooManyRequestException(baseResponse.Status, baseResponse.Message);
                case 500:
                    throw new UnexpectedException(baseResponse.Status, baseResponse.Message);
                default:
                    throw new InvalidOperationException($"something went wrong, httpStatus code: {response.StatusCode}, message: {response.RequestMessage}, please contact support team.");
            }
        }
    }
}
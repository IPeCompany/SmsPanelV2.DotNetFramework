using IPE.SmsIrClient.Exceptions;
using IPE.SmsIrClient.Models.Results;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPE.SmsIrClient.Handlers
{
    internal static class HttpResponseHandler
    {
        internal static async Task HandleUnsuccessfulResponseAsync(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            SmsIrResult baseResponse = JsonConvert.DeserializeObject<SmsIrResult>(content);
            int statusCode = (int)response.StatusCode;

            switch (statusCode)
            {
                case 401:
                    throw new UnauthorizedException(baseResponse.Status, baseResponse.Message);
                case 403:
                    throw new PlanAccessDeniedException(baseResponse.Status, baseResponse.Message);
                case 400:
                    throw new LogicalException(baseResponse.Status, baseResponse.Message);
                case 429:
                    throw new TooManyRequestException(baseResponse.Status, baseResponse.Message);
                case 500:
                    throw new UnexpectedException(baseResponse.Status, baseResponse.Message);
                default:
                    throw new InvalidOperationException($"Something went wrong, HttpStatusCode: {response.StatusCode}, Message: {response.RequestMessage}, please contact the support team.");
            }
        }

        internal static void HandleUnsuccessfulResponse(HttpResponseMessage response)
        {
            HandleUnsuccessfulResponseAsync(response).GetAwaiter().GetResult();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.Logic.Services.RequestSender
{
    public class RequestSender : IRequestSender
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RequestSender(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<ResponseModel> SendRequestAsync(RequestModel requestModel)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var httpClient = httpClientFactory.CreateClient();

                Stopwatch stopwatch = new Stopwatch();
                HttpRequestMessage request = new HttpRequestMessage(
                                                    TransformHttpMethod(requestModel.HttpMethod),
                                                    requestModel.Url);
                TryAddAuthenticationToRequest(requestModel.Authentication, request);
                TryAddContentToRequest(requestModel, request);

                startTime = DateTime.Now;
                stopwatch.Start();
                var response = await httpClient.SendAsync(request);
                stopwatch.Stop();
                DateTime endTime = DateTime.Now;

                return new ResponseModel()
                {
                    StatusCode = response.StatusCode,
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = stopwatch.Elapsed
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.GatewayTimeout,
                    StartTime = startTime,
                    EndTime = DateTime.Now,
                    ExceptionMessage = ex.Message
                };
            }
        }

        private System.Net.Http.HttpMethod TransformHttpMethod(Models.HttpMethod httpMethod)
        {
            return httpMethod switch
            {
                Models.HttpMethod.Get => System.Net.Http.HttpMethod.Get,
                Models.HttpMethod.Post => System.Net.Http.HttpMethod.Post,
                Models.HttpMethod.Put => System.Net.Http.HttpMethod.Put,
                Models.HttpMethod.Patch => System.Net.Http.HttpMethod.Patch,
                _ => throw new ArgumentException($"Httpmethod {httpMethod} not allowed."),
            };
        }

        public async Task<IEnumerable<ResponseModel>> SendRequestsAsync(RequestModel requestModel, int count, int perSecond)
        {
            var tasks = new List<Task<ResponseModel>>();

            int nrOfLoads = count / perSecond;
            for (int i = 0; i < nrOfLoads; i++)
            {
                for (int j = 0; j < perSecond; j++)
                {
                    tasks.Add(SendRequestAsync(requestModel));
                }
                await Task.Delay(1000);
            }

            await Task.WhenAll(tasks);
            var result = tasks.Select(t => t.Result);
            return result;
        }

        private void TryAddAuthenticationToRequest(AuthenticationModel authentication, HttpRequestMessage request)
        {
            if (authentication.AuthenticationType == AuthenticationType.BearerToken)
            {
                request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authentication.BearerToken);
            }
            else if (authentication.AuthenticationType == AuthenticationType.Basic)
            {
                string credentials = authentication.Username + ":" + authentication.Password;
                var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
                request.Headers.Authorization =
                        new AuthenticationHeaderValue("Basic", base64Credentials);
            }
        }

        private void TryAddContentToRequest(RequestModel requestModel, HttpRequestMessage request)
        {
            if (requestModel.HttpMethod == Models.HttpMethod.Post ||
                requestModel.HttpMethod == Models.HttpMethod.Put)
            {
                if (!string.IsNullOrEmpty(requestModel.JsonBody))
                {
                    request.Content = new StringContent(requestModel.JsonBody,
                                                        Encoding.UTF8,
                                                        "application/json");
                }
            }
        }
    }
}

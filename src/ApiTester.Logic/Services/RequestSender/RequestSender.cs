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
            var httpClient = httpClientFactory.CreateClient();

            Stopwatch stopwatch = new Stopwatch();
            HttpRequestMessage request = new HttpRequestMessage(
                                                TransformHttpMethod(requestModel.HttpMethod),
                                                requestModel.Url);
            TryAddAuthenticationToRequest(requestModel, request);
            TryAddContentToRequest(requestModel, request);

            stopwatch.Start();
            var response = await httpClient.SendAsync(request);
            stopwatch.Stop();

            return new ResponseModel()
            {
                StatusCode = response.StatusCode,
                Duration = stopwatch.Elapsed
            };
        }

        private System.Net.Http.HttpMethod TransformHttpMethod(Models.HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case Models.HttpMethod.Get:
                    return System.Net.Http.HttpMethod.Get;
                case Models.HttpMethod.Post:
                    return System.Net.Http.HttpMethod.Post;
                case Models.HttpMethod.Put:
                    return System.Net.Http.HttpMethod.Put;
                case Models.HttpMethod.Patch:
                    return System.Net.Http.HttpMethod.Patch;
                default:
                    throw new ArgumentException($"Httpmethod {httpMethod} not allowed.");
            }
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

        private void TryAddAuthenticationToRequest(RequestModel requestModel, HttpRequestMessage request)
        {
            if (requestModel.AuthenticationType == AuthenticationType.BearerToken)
            {
                request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", requestModel.BearerToken);
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

using System.Net.Http;

namespace ApiTester.Logic.Services.RequestSender.Models
{
    public class RequestModel
    {
        public string Url { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public string BearerToken { get; set; }
        public string JsonBody { get; set; }
    }
}

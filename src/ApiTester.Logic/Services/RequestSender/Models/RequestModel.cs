namespace ApiTester.Logic.Services.RequestSender.Models
{
    public class RequestModel
    {
        public string Url { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public AuthenticationModel Authentication { get; set; }
        public string JsonBody { get; set; }
    }
}

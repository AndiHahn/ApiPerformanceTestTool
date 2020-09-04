namespace ApiTester.Logic.Services.RequestSender.Models
{
    public class AuthenticationModel
    {
        public AuthenticationType AuthenticationType { get; set; }
        public string BearerToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

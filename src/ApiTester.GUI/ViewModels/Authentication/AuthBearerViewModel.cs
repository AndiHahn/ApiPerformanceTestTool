using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.GUI.ViewModels.Authentication
{
    public class AuthBearerViewModel : AuthViewModel
    {
        public string BearerToken { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1OTkyMzc5MzAsImV4cCI6MTU5OTMyNDMzMCwiaWF0IjoxNTk5MjM3OTMwfQ.7R2ybi0izsJ-_M5ds5IcJVYxB4IC06dU47k_FC282nk";

        public AuthBearerViewModel()
        {

        }

        public override AuthenticationModel GetAuthenticationModel()
        {
            return new AuthenticationModel()
            {
                AuthenticationType = AuthenticationType.BearerToken,
                BearerToken = BearerToken
            };
        }
    }
}

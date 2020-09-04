using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.GUI.ViewModels.Authentication
{
    public class AuthBearerViewModel : AuthViewModel
    {
        public string BearerToken { get; set; }

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

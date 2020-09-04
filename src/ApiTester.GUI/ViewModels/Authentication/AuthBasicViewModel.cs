using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.GUI.ViewModels.Authentication
{
    public class AuthBasicViewModel : AuthViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public AuthBasicViewModel()
        {

        }

        public override AuthenticationModel GetAuthenticationModel()
        {
            return new AuthenticationModel()
            {
                AuthenticationType = AuthenticationType.Basic,
                Username = Username,
                Password = Password
            };
        }
    }
}

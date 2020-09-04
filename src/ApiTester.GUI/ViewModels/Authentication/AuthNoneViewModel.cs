using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.GUI.ViewModels.Authentication
{
    public class AuthNoneViewModel : AuthViewModel
    {
        public override AuthenticationModel GetAuthenticationModel()
        {
            return new AuthenticationModel()
            {
                AuthenticationType = AuthenticationType.None
            };
        }
    }
}

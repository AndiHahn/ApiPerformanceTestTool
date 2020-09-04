using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.GUI.ViewModels.Authentication
{
    public abstract class AuthViewModel : ViewModel
    {
        public abstract AuthenticationModel GetAuthenticationModel();
    }
}

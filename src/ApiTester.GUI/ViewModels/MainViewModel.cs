using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ApiTester.GUI.Utilities;
using ApiTester.GUI.ViewModels.Authentication;
using ApiTester.Logic.Services.RequestSender;
using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.GUI.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IRequestSender requestSender;
        private AuthenticationType authenticationType;
        private AuthViewModel authenticationViewModel;

        public ICommand SendRequest { get; private set; }

        public string ApiUrl { get; set; }
        public string BearerToken { get; set; }
        public int NrOfRequestsToSend { get; set; } = 100;
        public int RequestsPerSecond { get; set; } = 50;
        public bool RequestSenderRunning { get; set; } = false;

        public AuthenticationType AuthenticationType
        {
            get => authenticationType; set
            {
                Set(ref authenticationType, value);
                switch (value)
                {
                    case AuthenticationType.None:
                        AuthenticationViewModel = new AuthNoneViewModel();
                        break;
                    case AuthenticationType.BearerToken:
                        AuthenticationViewModel = new AuthBearerViewModel();
                        break;
                    case AuthenticationType.Basic:
                        AuthenticationViewModel = new AuthBasicViewModel();
                        break;
                }
            }
        }

        public HttpMethod HttpMethod { get; set; }

        public IEnumerable<AuthenticationType> AuthenticationTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(AuthenticationType)).Cast<AuthenticationType>();
            }
        }

        public IEnumerable<HttpMethod> HttpMethodValues
        {
            get
            {
                return Enum.GetValues(typeof(HttpMethod)).Cast<HttpMethod>();
            }
        }

        public ResponseViewModel ResponseViewModel { get; set; }
        public AuthViewModel AuthenticationViewModel
        {
            get => authenticationViewModel; set
            {
                Set(ref authenticationViewModel, value);
            }
        }

        public MainViewModel(IRequestSender requestSender)
        {
            this.requestSender = requestSender ?? throw new ArgumentNullException(nameof(requestSender));
            ResponseViewModel = new ResponseViewModel();
            AuthenticationType = AuthenticationType.None;

            BindCommands();
        }


        private void BindCommands()
        {
            SendRequest = new AsyncDelegateCommand(SendRequestAsync, _ => !RequestSenderRunning);
        }

        private async Task SendRequestAsync(object arg)
        {
            try
            {
                RequestSenderRunning = true;
                InitResultFields();

                var requestModel = new RequestModel()
                {
                    Url = ApiUrl,
                    Authentication = AuthenticationViewModel.GetAuthenticationModel(),
                    HttpMethod = HttpMethod,
                    JsonBody = string.Empty
                };

                var result = await requestSender.SendRequestsAsync(requestModel, NrOfRequestsToSend, RequestsPerSecond);
                ResponseViewModel.Responses = new ObservableCollection<ResponseModel>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                RequestSenderRunning = false;
            }
        }

        private void InitResultFields()
        {
            ResponseViewModel.Responses = new ObservableCollection<ResponseModel>();
        }
    }
}

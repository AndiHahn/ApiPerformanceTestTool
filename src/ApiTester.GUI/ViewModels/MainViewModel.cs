using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ApiTester.GUI.Utilities;
using ApiTester.Logic.Services.RequestSender;
using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.GUI.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IRequestSender requestSender;

        public ICommand SendRequest { get; private set; }

        public string ApiUrl { get; set; } = "https://localhost:5001/api/bill?accountIds=1";
        public string BearerToken { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1OTg5NzQwMjUsImV4cCI6MTU5OTA2MDQyNSwiaWF0IjoxNTk4OTc0MDI1fQ.MbD-YDN5yHsbYHxkx6ZjHK-jwejSDx7GC6i2aVcLIY4";
        public int NrOfRequestsToSend { get; set; } = 100;
        public int RequestsPerSecond { get; set; } = 50;

        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.BearerToken;
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

        public MainViewModel(IRequestSender requestSender)
        {
            this.requestSender = requestSender ?? throw new ArgumentNullException(nameof(requestSender));
            ResponseViewModel = new ResponseViewModel();

            BindCommands();
        }


        private void BindCommands()
        {
            SendRequest = new AsyncDelegateCommand(SendRequestAsync, _ => true);
        }

        private async Task SendRequestAsync(object arg)
        {
            try
            {
                var requestModel = new RequestModel()
                {
                    Url = ApiUrl,
                    AuthenticationType = AuthenticationType,
                    BearerToken = BearerToken,
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
        }
    }
}

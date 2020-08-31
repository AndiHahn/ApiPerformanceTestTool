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
        private int statusCode;
        private TimeSpan timeSpan;
        private AuthenticationType authenticationType;
        private HttpMethod httpMethod;
        private ObservableCollection<ResponseModel> responses;

        public ICommand SendRequest { get; private set; }

        public string ApiUrl { get; set; }
        public string BearerToken { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
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

        public ObservableCollection<ResponseModel> Responses
        {
            get => responses; set
            {
                this.Set(ref responses, value);
            }
        }

        public MainViewModel(IRequestSender requestSender)
        {
            this.requestSender = requestSender ?? throw new ArgumentNullException(nameof(requestSender));

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

                var result = await requestSender.SendRequestsAsync(requestModel, 1000, 1000);
                Responses = new ObservableCollection<ResponseModel>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}

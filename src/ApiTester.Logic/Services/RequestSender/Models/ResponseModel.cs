using System;
using System.Net;

namespace ApiTester.Logic.Services.RequestSender.Models
{
    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public TimeSpan Duration { get; set; }
    }
}

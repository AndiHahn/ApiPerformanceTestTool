using System;
using System.Net;

namespace ApiTester.Logic.Services.RequestSender.Models
{
    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string ExceptionMessage { get; set; }
    }
}

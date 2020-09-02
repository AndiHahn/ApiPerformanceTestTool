using System.Net;

namespace ApiTester.Logic.Models
{
    public class StatisticModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public int NrOfResponses { get; set; }

        public StatisticModel(HttpStatusCode statusCode, int nrOfResponses)
        {
            StatusCode = statusCode;
            NrOfResponses = nrOfResponses;
        }

        public StatisticModel()
        {

        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.Logic.Services.RequestSender
{
    public interface IRequestSender
    {
        Task<ResponseModel> SendRequestAsync(RequestModel requestModel);
        Task<IEnumerable<ResponseModel>> SendRequestsAsync(RequestModel requestModel, int count, int perSecond);
    }
}

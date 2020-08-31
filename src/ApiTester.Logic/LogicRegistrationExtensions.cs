using ApiTester.Logic.Services.RequestSender;
using Microsoft.Extensions.DependencyInjection;

namespace ApiTester.Logic
{
    public static class LogicRegistrationExtensions
    {
        public static IServiceCollection RegisterLogic(this IServiceCollection services)
        {
            services.AddScoped<IRequestSender, RequestSender>();

            return services;
        }
    }
}

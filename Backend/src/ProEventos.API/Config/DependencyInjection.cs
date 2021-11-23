using Microsoft.Extensions.DependencyInjection;
using ProEventos.Application.Contracts;
using ProEventos.Application.Implementations;
using ProEventos.Persistence.Contracts;
using ProEventos.Persistence.Implementations;

namespace ProEventos.API.Config
{
    public static class DependencyInjection
    {
        public static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IBatchService, BatchService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        public static void InjectPersistences(IServiceCollection services)
        {
            services.AddScoped<IGeneralPersist, GeneralPersist>();
            services.AddScoped<IEventPersist, EventPersist>();
            services.AddScoped<IBatchPersist, BatchPersist>();
            services.AddScoped<IUserPersist, UserPersist>();
        }
    }
}

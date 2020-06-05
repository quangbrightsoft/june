using Microsoft.Extensions.DependencyInjection;

namespace JuneApp.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            #region Account

            services.AddScoped<IAccountService, AccountService>();

            #endregion

            #region HttpContext

            services.AddScoped<IUserService, UserService>();

            #endregion

            return services;
        }
    }
}

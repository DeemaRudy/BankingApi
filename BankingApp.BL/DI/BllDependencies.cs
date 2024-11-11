using BankingApp.BLL.Services;
using BankingApp.BLL.Services.Implementation;
using BankingApp.DAL.DI;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.BLL.DI
{
    public static class BllDependencies
    {
        public static IServiceCollection RegisterBllDependencies(
             this IServiceCollection services)
        {
            services.RegisterDalDependencies();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountManagementService, AccountManagementService>();
            services.AddScoped<IAccountTransactionsService, AccountTransactionsService>();

            return services;
        }
    }
}

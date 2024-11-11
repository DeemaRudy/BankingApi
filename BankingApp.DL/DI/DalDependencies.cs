using BankingApp.DAL.EF;
using BankingApp.DAL.Repositories;
using BankingApp.DAL.Repositories.Implementation;
using BankingApp.DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWorkImplementation = BankingApp.DAL.UnitOfWork.Implementation.UnitOfWork;

namespace BankingApp.DAL.DI
{
    public static class DalDependencies
    {
        public static IServiceCollection RegisterDalDependencies(
             this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWorkImplementation>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped(typeof(BankingAppContext));

            return services;
        }
    }
}

using BankingApp.BLL.DI;

namespace BankingApp.API.DI
{
    public static class ApiDependencies
    {
        public static IServiceCollection RegisterDependencies(
                this IServiceCollection services)
        {
            services.RegisterBllDependencies();

            return services;
        }
    }
}

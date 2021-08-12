using Doppler.BigQueryMicroservice.Repository;
using Doppler.BigQueryMicroservice.Repository.Classes;
using Doppler.BigQueryMicroservice.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Doppler.BigQueryMicroservice.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUserAccessByUserRepository, UserAccessByUserRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDatabaseConnectionFactory, DatabaseConnectionFactory>();

        }
    }
}

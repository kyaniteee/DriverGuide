using DriverGuide.Domain.Interfaces.Repositories;
using DriverGuide.Infrastructure.Core.Repository;
using DriverGuide.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DriverGuide.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration)
        {
            // Rejestracja DbContext
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Rejestracja repozytoriów
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IPytanieRepository, PytanieRepository>();
            services.AddScoped<IOdpowiedzRepository, OdpowiedzRepository>();

            // Rejestracja mapperów
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddAutoMapper(typeof(OdpowiedzProfile).Assembly);
            services.AddAutoMapper(typeof(PytanieProfile).Assembly);
            services.AddAutoMapper(typeof(RoleProfile).Assembly);
            services.AddAutoMapper(typeof(TestProfile).Assembly);

            return services;
        }
    }
}

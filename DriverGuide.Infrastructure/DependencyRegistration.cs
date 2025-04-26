using DriverGuide.Domain.Interfaces.Repositories;
using DriverGuide.Infrastructure.Database;
using DriverGuide.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DriverGuide.Infrastructure
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Rejestracja DbContext
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Rejestracja repozytoriów
            services.AddScoped<IUserRepository, UserSqlRepository>();
            services.AddScoped<ITestRepository, TestSqlRepository>();
            services.AddScoped<IPytanieRepository, PytanieSqlRepository>();
            services.AddScoped<IOdpowiedzRepository, OdpowiedzSqlRepository>();

            // Rejestracja mapperów
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddAutoMapper(typeof(RoleProfile).Assembly);
            services.AddAutoMapper(typeof(TestProfile).Assembly);
            services.AddAutoMapper(typeof(PytanieProfile).Assembly);
            services.AddAutoMapper(typeof(OdpowiedzProfile).Assembly);

            return services;
        }
    }
}

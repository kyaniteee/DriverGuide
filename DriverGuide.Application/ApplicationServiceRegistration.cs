using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DriverGuide.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Rejestracja wszystkich walidatorów z assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            return services;
        }
    }
}

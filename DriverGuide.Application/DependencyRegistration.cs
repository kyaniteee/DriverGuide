using DriverGuide.Application.Commands;
using DriverGuide.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DriverGuide.Application
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateQuestionFileCommand).Assembly));

            // Rejestracja wszystkich walidatorów z assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //Rejestracja uwieżytelnienia jwt
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}

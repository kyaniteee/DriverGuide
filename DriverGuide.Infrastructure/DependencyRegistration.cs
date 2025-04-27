using DriverGuide.Domain.Interfaces;
using DriverGuide.Infrastructure.Database;
using DriverGuide.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DriverGuide.Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Rejestracja DbContext
        services.AddDbContext<DriverGuideDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Rejestracja repozytoriów
        services.AddScoped<IUserRepository, UserSqlRepository>();
        services.AddScoped<ITestSessionRepository, TestSqlRepository>();
        services.AddScoped<IQuestionRepository, QuestionSqlRepository>();
        services.AddScoped<IQuestionAnswerRepository, QuestionAnswerSqlRepository>();

        // Rejestracja mapperów
        services.AddAutoMapper(typeof(UserProfile).Assembly);
        services.AddAutoMapper(typeof(RoleProfile).Assembly);
        services.AddAutoMapper(typeof(TestProfile).Assembly);
        services.AddAutoMapper(typeof(QuestionProfile).Assembly);
        services.AddAutoMapper(typeof(QuestionAnswerProfile).Assembly);

        return services;
    }
}

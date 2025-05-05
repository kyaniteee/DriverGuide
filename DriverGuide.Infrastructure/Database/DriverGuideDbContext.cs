using DriverGuide.Domain.Models;
using DriverGuide.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DriverGuide.Infrastructure.Database
{
    public class DriverGuideDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {

        public DriverGuideDbContext(DbContextOptions<DriverGuideDbContext> options) : base(options) { }

        public DbSet<Question>? Questions { get; set; }
        public DbSet<QuestionAnswer>? QuestionAnswers { get; set; }
        public DbSet<TestSession>? TestSessions { get; set; }
        public DbSet<QuestionFile>? QuestionFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001").ToString(),

            },
            new()
            {
                Name = "User",
                NormalizedName = "USER",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002").ToString(),
            });

            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new TestSessionConfiguration());
            builder.ApplyConfiguration(new QuestionFileConfiguration());
            builder.ApplyConfiguration(new QuestionAnswerConfiguration());

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

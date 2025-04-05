using DriverGuide.Data.Configurations;
using DriverGuide.Domain.Entities;
using DriverGuide.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DriverGuide.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pytanie>? Questions { get; set; }
        public DbSet<Odpowiedz>? Answers { get; set; }
        public DbSet<Test>? Tests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Name = "User",
                NormalizedName = "USER"
            });

            builder.ApplyConfiguration(new TestConfigurations());
            builder.ApplyConfiguration(new PytanieConfigurations());
            builder.ApplyConfiguration(new OdpowiedzConfigurations());

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

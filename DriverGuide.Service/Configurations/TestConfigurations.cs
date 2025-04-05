using DriverGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverGuide.Infrastructure.Configurations
{
    public class TestConfigurations : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("Testy");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.DataOd).IsRequired().HasColumnName(nameof(Test.DataOd));
            builder.Property(a => a.Wynik).IsRequired(false).HasColumnName(nameof(Test.Wynik));
            builder.Property(a => a.DataDo).IsRequired(false).HasColumnName(nameof(Test.DataDo));
            builder.Property(a => a.UzytkownikId).IsRequired().HasColumnName(nameof(Test.UzytkownikId));
        }
    }
}

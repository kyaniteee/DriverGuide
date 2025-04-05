using DriverGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverGuide.Infrastructure.Configurations
{
    public class OdpowiedzConfigurations : IEntityTypeConfiguration<Odpowiedz>
    {
        public void Configure(EntityTypeBuilder<Odpowiedz> builder)
        {
            builder.ToTable("Odpowiedzi");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.TestId).IsRequired().HasColumnName(nameof(Odpowiedz.TestId));
            builder.Property(a => a.PytanieId).IsRequired().HasColumnName(nameof(Odpowiedz.PytanieId));
            builder.Property(a => a.Poprawna).IsRequired().HasColumnName(nameof(Odpowiedz.Poprawna));
            builder.Property(a => a.Tekst).IsRequired().HasMaxLength(512).HasColumnName(nameof(Odpowiedz.Tekst));
        }
    }
}

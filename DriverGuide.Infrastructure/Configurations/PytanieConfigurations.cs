using DriverGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverGuide.Infrastructure.Configurations
{
    public class PytanieConfigurations : IEntityTypeConfiguration<Pytanie>
    {
        public void Configure(EntityTypeBuilder<Pytanie> builder)
        {
            builder.ToTable("Pytania");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.NumerPytania).HasColumnName(nameof(Pytanie.NumerPytania));
            builder.Property(a => a.Tresc).HasColumnName(nameof(Pytanie.Tresc));
            builder.Property(a => a.OdpA).HasColumnName(nameof(Pytanie.OdpA));
            builder.Property(a => a.OdpB).HasColumnName(nameof(Pytanie.OdpB));
            builder.Property(a => a.OdpC).HasColumnName(nameof(Pytanie.OdpC));
            builder.Property(a => a.PoprawnaOdp).HasColumnName(nameof(Pytanie.PoprawnaOdp));
            builder.Property(a => a.Media).HasColumnName(nameof(Pytanie.Media));
            builder.Property(a => a.Kategorie).HasColumnName(nameof(Pytanie.Kategorie));
            builder.Property(a => a.MediaMigowe).HasColumnName(nameof(Pytanie.MediaMigowe));
            builder.Property(a => a.TrescPyt).HasColumnName(nameof(Pytanie.TrescPyt));
            builder.Property(a => a.PytanieENG).HasColumnName(nameof(Pytanie.PytanieENG));
            builder.Property(a => a.OdpAENG).HasColumnName(nameof(Pytanie.OdpAENG));
            builder.Property(a => a.OdpBENG).HasColumnName(nameof(Pytanie.OdpBENG));
            builder.Property(a => a.OdpCENG).HasColumnName(nameof(Pytanie.OdpCENG));
            builder.Property(a => a.PytanieDE).HasColumnName(nameof(Pytanie.PytanieDE));
            builder.Property(a => a.OdpADE).HasColumnName(nameof(Pytanie.OdpADE));
            builder.Property(a => a.OdpBDE).HasColumnName(nameof(Pytanie.OdpBDE));
            builder.Property(a => a.OdpCDE).HasColumnName(nameof(Pytanie.OdpCDE));
            builder.Property(a => a.PytanieUA).HasColumnName(nameof(Pytanie.PytanieUA));
            builder.Property(a => a.OdpAUA).HasColumnName(nameof(Pytanie.OdpAUA));
            builder.Property(a => a.OdpBUA).HasColumnName(nameof(Pytanie.OdpBUA));
            builder.Property(a => a.OdpCUA).HasColumnName(nameof(Pytanie.OdpCUA));
        }
    }
}

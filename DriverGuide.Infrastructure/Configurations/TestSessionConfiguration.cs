using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverGuide.Infrastructure.Configurations;

public class TestSessionConfiguration : IEntityTypeConfiguration<TestSession>
{
    public void Configure(EntityTypeBuilder<TestSession> builder)
    {
        builder.ToTable("TestSessions");
        builder.HasKey(t => t.TestSessionId);

        builder.Property(t => t.TestSessionId)
            .HasColumnName(nameof(TestSession.TestSessionId))
            .IsRequired();

        builder.Property(t => t.UserId)
            .HasColumnName(nameof(TestSession.UserId))
            .IsRequired(false);

        builder.Property(t => t.StartDate)
            .HasColumnName(nameof(TestSession.StartDate))
            .IsRequired();

        builder.Property(t => t.EndDate)
            .IsRequired(false)
            .HasColumnName(nameof(TestSession.EndDate));

        builder.Property(t => t.Result)
            .IsRequired(false)
            .HasColumnName(nameof(TestSession.Result));

        builder.HasIndex(t => t.UserId)
               .IsUnique(false);
    }
}

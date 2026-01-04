using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverGuide.Infrastructure.Configurations;

public class QuestionFileConfiguration : IEntityTypeConfiguration<QuestionFile>
{
    public void Configure(EntityTypeBuilder<QuestionFile> builder)
    {
        builder.ToTable("QuestionFiles");
        builder.HasKey(t => t.QuestionFileId);
        builder.Property(t => t.QuestionFileId)
            .HasColumnName(nameof(QuestionFile.QuestionFileId));

        builder.Property(t => t.UploadDate)
            .HasColumnName(nameof(QuestionFile.UploadDate))
            .HasDefaultValue(DateOnly.FromDateTime(DateTime.Now));

        builder.Property(t => t.ContentType)
           .HasColumnName(nameof(QuestionFile.ContentType))
           .IsRequired(true);

        builder.Property(t => t.Name)
            .HasColumnName(nameof(QuestionFile.Name))
            .IsRequired(true);

        builder.Property(t => t.File)
            .IsRequired(true)
            .HasColumnName(nameof(QuestionFile.File));

        builder.HasIndex(t => t.Name)
            .IsUnique()
            .HasDatabaseName("IX_QuestionFiles_Name");
    }
}

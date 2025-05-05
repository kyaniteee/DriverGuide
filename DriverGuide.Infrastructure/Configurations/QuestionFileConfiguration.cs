using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverGuide.Infrastructure.Configurations;

public class QuestionFileConfiguration : IEntityTypeConfiguration<QuestionFile>
{
    public void Configure(EntityTypeBuilder<QuestionFile> builder)
    {
        builder.ToTable("QuestionFiles");
        builder.HasKey(t => t.QuestionAttachmentId);
        builder.Property(t => t.QuestionAttachmentId)
            .HasColumnName(nameof(QuestionFile.QuestionAttachmentId));

        builder.Property(t => t.UploadDate)
            .HasColumnName(nameof(QuestionFile.UploadDate))
            .HasDefaultValue(DateOnly.FromDateTime(DateTime.Now));

        builder.Property(t => t.FileMimeType)
           .HasColumnName(nameof(QuestionFile.FileMimeType))
           .IsRequired(true);

        builder.Property(t => t.FileName)
            .HasColumnName(nameof(QuestionFile.FileName))
            .IsRequired(true);

        builder.Property(t => t.File)
            .IsRequired(true)
            .HasColumnName(nameof(QuestionFile.File));
    }
}

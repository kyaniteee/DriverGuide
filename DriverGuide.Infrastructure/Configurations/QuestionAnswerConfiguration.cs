using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverGuide.Infrastructure.Configurations;

public class QuestionAnswerConfiguration : IEntityTypeConfiguration<QuestionAnswer>
{
    public void Configure(EntityTypeBuilder<QuestionAnswer> builder)
    {
        builder.ToTable("QuestionAnswers");
        builder.HasKey(a => a.QuestionAnswerId);
        
        builder.Property(qa => qa.QuestionAnswerId)
            .HasColumnName(nameof(QuestionAnswer.QuestionAnswerId));

        builder.Property(qa => qa.TestSessionId)
            .HasColumnName(nameof(QuestionAnswer.TestSessionId))
            .IsRequired(false);

        builder.Property(qa => qa.QuestionId)
            .HasColumnName(nameof(QuestionAnswer.QuestionId));

        builder.Property(qa => qa.QuestionCategory) 
            .HasColumnName(nameof(QuestionAnswer.QuestionCategory))
            .IsRequired()
            .HasConversion<int>();

        builder.Property(qa => qa.Question)
            .HasColumnName(nameof(QuestionAnswer.Question));

        builder.Property(qa => qa.CorrectQuestionAnswer)
            .HasColumnName(nameof(QuestionAnswer.CorrectQuestionAnswer));

        builder.Property(qa => qa.UserQuestionAnswer)
            .IsRequired(false)
            .HasColumnName(nameof(QuestionAnswer.UserQuestionAnswer));

        builder.Property(qa => qa.StartDate)
            .HasColumnName(nameof(QuestionAnswer.StartDate));

        builder.Property(qa => qa.EndDate)
            .IsRequired(false)
            .HasColumnName(nameof(QuestionAnswer.EndDate));

        builder.Property(qa => qa.QuestionLanguage)
            .HasColumnName(nameof(QuestionAnswer.QuestionLanguage))
            .HasConversion<string>() 
            .HasMaxLength(3);

        builder.HasOne(qa => qa.TestSession)
            .WithMany(ts => ts.QuestionAnswers)
            .HasForeignKey(qa => qa.TestSessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_QuestionAnswers_TestSessions");
    }
}

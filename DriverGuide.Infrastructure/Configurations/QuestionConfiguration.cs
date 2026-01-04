using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverGuide.Infrastructure.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions"); 
        builder.HasKey(q => q.QuestionId); 

        builder.Property(q => q.QuestionId)
            .HasColumnName(nameof(Question.QuestionId));

        builder.Property(q => q.Points)
            .HasColumnName(nameof(Question.Points))
            .HasDefaultValue(1);

        builder.Property(q => q.TimeToAnswerSeconds)
            .HasColumnName(nameof(Question.TimeToAnswerSeconds))
            .HasDefaultValue(30);

        builder.Property(q => q.IsGeneral)
            .HasColumnName(nameof(Question.IsGeneral))
            .HasDefaultValue(true);

        builder.Property(q => q.DataDodania)
            .HasColumnName(nameof(Question.DataDodania));

        builder.Property(q => q.Lp)
            .HasColumnName(nameof(Question.Lp));

        builder.Property(q => q.NumerPytania)
            .HasColumnName(nameof(Question.NumerPytania));

        builder.Property(q => q.Pytanie)
            .HasColumnName(nameof(Question.Pytanie));

        builder.Property(q => q.OdpowiedzA)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzA));

        builder.Property(q => q.OdpowiedzB)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzB));

        builder.Property(q => q.OdpowiedzC)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzC));

        builder.Property(q => q.PoprawnaOdp)
            .IsRequired(false)
            .HasColumnName(nameof(Question.PoprawnaOdp));

        builder.Property(q => q.Media)
            .IsRequired(false)
            .HasColumnName(nameof(Question.Media));

        builder.Property(q => q.Kategorie)
            .IsRequired(false)
            .HasColumnName(nameof(Question.Kategorie));

        builder.Property(q => q.NazwaMediaTlumaczenieMigowePJMtrescPyt)
            .IsRequired(false)
            .HasColumnName(nameof(Question.NazwaMediaTlumaczenieMigowePJMtrescPyt));

        builder.Property(q => q.NazwaMediaTlumaczenieMigowePJMtrescA)
            .IsRequired(false)
            .HasColumnName(nameof(Question.NazwaMediaTlumaczenieMigowePJMtrescA));

        builder.Property(q => q.NazwaMediaTlumaczenieMigowePJMtrescB)
            .IsRequired(false)
            .HasColumnName(nameof(Question.NazwaMediaTlumaczenieMigowePJMtrescB));

        builder.Property(q => q.NazwaMediaTlumaczenieMigowePJMtrescC)
            .HasColumnName(nameof(Question.NazwaMediaTlumaczenieMigowePJMtrescC));

        builder.Property(q => q.PytanieENG)
            .IsRequired(false)
            .HasColumnName(nameof(Question.PytanieENG));

        builder.Property(q => q.OdpowiedzAENG)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzAENG));

        builder.Property(q => q.OdpowiedzBENG)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzBENG));

        builder.Property(q => q.OdpowiedzCENG)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzCENG));

        builder.Property(q => q.PytanieDE)
            .IsRequired(false)
            .HasColumnName(nameof(Question.PytanieDE));

        builder.Property(q => q.OdpowiedzADE)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzADE));

        builder.Property(q => q.OdpowiedzBDE)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzBDE));

        builder.Property(q => q.OdpowiedzCDE)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzCDE));

        builder.Property(q => q.PytanieUA)
            .IsRequired(false)
            .HasColumnName(nameof(Question.PytanieUA));

        builder.Property(q => q.OdpowiedzAUA)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzAUA));

        builder.Property(q => q.OdpowiedzBUA)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzBUA));

        builder.Property(q => q.OdpowiedzCUA)
            .IsRequired(false)
            .HasColumnName(nameof(Question.OdpowiedzCUA));

        builder.HasIndex(q => q.Media)
            .HasDatabaseName("IX_Questions_Media");
    }
}

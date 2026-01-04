using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;
using FluentAssertions;

namespace DriverGuide.Tests.Domain.Models;

public class BulkAnswerItemTests
{
    [Fact]
    public void BulkAnswerItem_ShouldHaveDefaultValues()
    {
        var bulkAnswer = new BulkAnswerItem();

        bulkAnswer.QuestionId.Should().Be(0);
        bulkAnswer.Question.Should().Be(string.Empty);
        bulkAnswer.CorrectQuestionAnswer.Should().Be(string.Empty);
        bulkAnswer.UserQuestionAnswer.Should().Be(string.Empty);
    }

    [Fact]
    public void BulkAnswerItem_ShouldAllowSettingAllProperties()
    {
        var startDate = DateTimeOffset.Now;
        var endDate = DateTimeOffset.Now.AddSeconds(20);
        
        var bulkAnswer = new BulkAnswerItem
        {
            QuestionId = 123,
            QuestionCategory = LicenseCategory.B,
            Question = "Test pytanie?",
            CorrectQuestionAnswer = "A",
            UserQuestionAnswer = "A",
            StartDate = startDate,
            EndDate = endDate,
            QuestionLanguage = Language.PL
        };

        bulkAnswer.QuestionId.Should().Be(123);
        bulkAnswer.QuestionCategory.Should().Be(LicenseCategory.B);
        bulkAnswer.Question.Should().Be("Test pytanie?");
        bulkAnswer.CorrectQuestionAnswer.Should().Be("A");
        bulkAnswer.UserQuestionAnswer.Should().Be("A");
        bulkAnswer.StartDate.Should().Be(startDate);
        bulkAnswer.EndDate.Should().Be(endDate);
        bulkAnswer.QuestionLanguage.Should().Be(Language.PL);
    }

    [Fact]
    public void BulkAnswerItem_CorrectAnswer_ShouldMatchUserAnswer()
    {
        var bulkAnswer = new BulkAnswerItem
        {
            CorrectQuestionAnswer = "B",
            UserQuestionAnswer = "B"
        };

        bulkAnswer.UserQuestionAnswer.Should().Be(bulkAnswer.CorrectQuestionAnswer);
    }

    [Fact]
    public void BulkAnswerItem_IncorrectAnswer_ShouldNotMatchUserAnswer()
    {
        var bulkAnswer = new BulkAnswerItem
        {
            CorrectQuestionAnswer = "A",
            UserQuestionAnswer = "C"
        };

        bulkAnswer.UserQuestionAnswer.Should().NotBe(bulkAnswer.CorrectQuestionAnswer);
    }

    [Theory]
    [InlineData(LicenseCategory.A)]
    [InlineData(LicenseCategory.B)]
    [InlineData(LicenseCategory.C)]
    [InlineData(LicenseCategory.D)]
    public void BulkAnswerItem_ShouldAcceptAllLicenseCategories(LicenseCategory category)
    {
        var bulkAnswer = new BulkAnswerItem
        {
            QuestionCategory = category
        };

        bulkAnswer.QuestionCategory.Should().Be(category);
    }

    [Theory]
    [InlineData(Language.PL)]
    [InlineData(Language.ENG)]
    [InlineData(Language.DE)]
    [InlineData(Language.UA)]
    public void BulkAnswerItem_ShouldAcceptAllLanguages(Language language)
    {
        var bulkAnswer = new BulkAnswerItem
        {
            QuestionLanguage = language
        };

        bulkAnswer.QuestionLanguage.Should().Be(language);
    }

    [Fact]
    public void BulkAnswerItem_TimeSpan_ShouldBeCalculatable()
    {
        var startDate = DateTimeOffset.Now;
        var endDate = startDate.AddSeconds(25);
        
        var bulkAnswer = new BulkAnswerItem
        {
            StartDate = startDate,
            EndDate = endDate
        };

        var timeSpent = bulkAnswer.EndDate - bulkAnswer.StartDate;
        timeSpent.TotalSeconds.Should().Be(25);
    }
}

using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;
using FluentAssertions;

namespace DriverGuide.Tests.Domain.Models;

public class QuestionAnswerTests
{
    [Fact]
    public void QuestionAnswer_ShouldHaveDefaultValues()
    {
        var questionAnswer = new QuestionAnswer();

        questionAnswer.QuestionAnswerId.Should().BeNull();
        questionAnswer.TestSessionId.Should().BeNull();
        questionAnswer.QuestionId.Should().BeNull();
        questionAnswer.UserQuestionAnswer.Should().BeNull();
        questionAnswer.EndDate.Should().BeNull();
    }

    [Fact]
    public void QuestionAnswer_ShouldAllowSettingAllProperties()
    {
        var startDate = DateTimeOffset.Now;
        var endDate = DateTimeOffset.Now.AddMinutes(1);
        
        var questionAnswer = new QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = "123",
            QuestionCategory = LicenseCategory.B,
            Question = "Test pytanie?",
            CorrectQuestionAnswer = "A",
            UserQuestionAnswer = "A",
            StartDate = startDate,
            EndDate = endDate,
            QuestionLanguage = Language.PL
        };

        questionAnswer.QuestionAnswerId.Should().NotBeNullOrEmpty();
        questionAnswer.TestSessionId.Should().NotBeNullOrEmpty();
        questionAnswer.QuestionId.Should().Be("123");
        questionAnswer.QuestionCategory.Should().Be(LicenseCategory.B);
        questionAnswer.Question.Should().Be("Test pytanie?");
        questionAnswer.CorrectQuestionAnswer.Should().Be("A");
        questionAnswer.UserQuestionAnswer.Should().Be("A");
        questionAnswer.StartDate.Should().Be(startDate);
        questionAnswer.EndDate.Should().Be(endDate);
        questionAnswer.QuestionLanguage.Should().Be(Language.PL);
    }

    [Fact]
    public void QuestionAnswer_CorrectAnswer_ShouldMatchUserAnswer()
    {
        var questionAnswer = new QuestionAnswer
        {
            CorrectQuestionAnswer = "B",
            UserQuestionAnswer = "B"
        };

        questionAnswer.UserQuestionAnswer.Should().Be(questionAnswer.CorrectQuestionAnswer);
    }

    [Fact]
    public void QuestionAnswer_IncorrectAnswer_ShouldNotMatchUserAnswer()
    {
        var questionAnswer = new QuestionAnswer
        {
            CorrectQuestionAnswer = "A",
            UserQuestionAnswer = "B"
        };

        questionAnswer.UserQuestionAnswer.Should().NotBe(questionAnswer.CorrectQuestionAnswer);
    }

    [Fact]
    public void QuestionAnswer_Language_ShouldAcceptAllLanguages()
    {
        var questionAnswer1 = new QuestionAnswer { QuestionLanguage = Language.PL };
        var questionAnswer2 = new QuestionAnswer { QuestionLanguage = Language.ENG };
        var questionAnswer3 = new QuestionAnswer { QuestionLanguage = Language.DE };
        var questionAnswer4 = new QuestionAnswer { QuestionLanguage = Language.UA };

        questionAnswer1.QuestionLanguage.Should().Be(Language.PL);
        questionAnswer2.QuestionLanguage.Should().Be(Language.ENG);
        questionAnswer3.QuestionLanguage.Should().Be(Language.DE);
        questionAnswer4.QuestionLanguage.Should().Be(Language.UA);
    }

    [Fact]
    public void QuestionAnswer_Category_ShouldAcceptAllCategories()
    {
        var questionAnswer = new QuestionAnswer { QuestionCategory = LicenseCategory.C };

        questionAnswer.QuestionCategory.Should().Be(LicenseCategory.C);
    }
}

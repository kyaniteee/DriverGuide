using DriverGuide.Domain.Models;
using FluentAssertions;

namespace DriverGuide.Tests.Domain.Models;

public class QuestionTests
{
    [Fact]
    public void Question_ShouldHaveDefaultValues()
    {
        var question = new Question();

        question.QuestionId.Should().Be(0);
        question.Points.Should().Be(0);
        question.TimeToAnswerSeconds.Should().Be(0);
        question.IsGeneral.Should().BeFalse();
    }

    [Fact]
    public void Question_ShouldAllowSettingProperties()
    {
        var question = new Question
        {
            QuestionId = 1,
            Points = 3,
            TimeToAnswerSeconds = 30,
            IsGeneral = true,
            Pytanie = "Test pytanie?",
            OdpowiedzA = "Odpowiedü A",
            OdpowiedzB = "Odpowiedü B",
            OdpowiedzC = "Odpowiedü C",
            PoprawnaOdp = "A",
            Kategorie = "B,C",
            Media = "test.jpg"
        };

        question.QuestionId.Should().Be(1);
        question.Points.Should().Be(3);
        question.TimeToAnswerSeconds.Should().Be(30);
        question.IsGeneral.Should().BeTrue();
        question.Pytanie.Should().Be("Test pytanie?");
        question.OdpowiedzA.Should().Be("Odpowiedü A");
        question.OdpowiedzB.Should().Be("Odpowiedü B");
        question.OdpowiedzC.Should().Be("Odpowiedü C");
        question.PoprawnaOdp.Should().Be("A");
        question.Kategorie.Should().Be("B,C");
        question.Media.Should().Be("test.jpg");
    }

    [Fact]
    public void Question_MultilingualProperties_ShouldBeIndependent()
    {
        var question = new Question
        {
            Pytanie = "Pytanie po polsku?",
            PytanieENG = "Question in English?",
            PytanieDE = "Frage auf Deutsch?",
            PytanieUA = "??????? ????????????"
        };

        question.Pytanie.Should().Be("Pytanie po polsku?");
        question.PytanieENG.Should().Be("Question in English?");
        question.PytanieDE.Should().Be("Frage auf Deutsch?");
        question.PytanieUA.Should().Be("??????? ????????????");
    }

    [Fact]
    public void Question_MultilingualAnswers_ShouldBeIndependent()
    {
        var question = new Question
        {
            OdpowiedzA = "Tak",
            OdpowiedzAENG = "Yes",
            OdpowiedzADE = "Ja",
            OdpowiedzAUA = "???"
        };

        question.OdpowiedzA.Should().Be("Tak");
        question.OdpowiedzAENG.Should().Be("Yes");
        question.OdpowiedzADE.Should().Be("Ja");
        question.OdpowiedzAUA.Should().Be("???");
    }
}

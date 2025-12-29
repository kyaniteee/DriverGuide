using DriverGuide.Domain.Enums;
using FluentAssertions;

namespace DriverGuide.Tests.Domain.Enums;

public class LanguageTests
{
    [Fact]
    public void Language_ShouldContainAllLanguages()
    {
        var allLanguages = Enum.GetValues<Language>();

        allLanguages.Should().Contain(Language.PL);
        allLanguages.Should().Contain(Language.ENG);
        allLanguages.Should().Contain(Language.DE);
        allLanguages.Should().Contain(Language.UA);
    }

    [Theory]
    [InlineData(Language.PL, "PL")]
    [InlineData(Language.ENG, "ENG")]
    [InlineData(Language.DE, "DE")]
    [InlineData(Language.UA, "UA")]
    public void Language_ToString_ShouldReturnLanguageCode(Language language, string expected)
    {
        var result = language.ToString();

        result.Should().Be(expected);
    }

    [Fact]
    public void Language_Parse_ShouldConvertFromString()
    {
        var languageString = "PL";

        var language = Enum.Parse<Language>(languageString);

        language.Should().Be(Language.PL);
    }

    [Fact]
    public void Language_TryParse_InvalidLanguage_ShouldReturnFalse()
    {
        var languageString = "FR";

        var success = Enum.TryParse<Language>(languageString, out var language);

        success.Should().BeFalse();
    }

    [Theory]
    [InlineData("pl", Language.PL)]
    [InlineData("eng", Language.ENG)]
    [InlineData("de", Language.DE)]
    [InlineData("ua", Language.UA)]
    public void Language_Parse_CaseInsensitive_ShouldWork(string languageString, Language expected)
    {
        var success = Enum.TryParse<Language>(languageString, true, out var language);

        success.Should().BeTrue();
        language.Should().Be(expected);
    }
}

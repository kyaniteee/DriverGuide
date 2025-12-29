using DriverGuide.Domain.Enums;
using FluentAssertions;

namespace DriverGuide.Tests.Domain.Enums;

public class LicenseCategoryTests
{
    [Fact]
    public void LicenseCategory_ShouldContainAllCategories()
    {
        var allCategories = Enum.GetValues<LicenseCategory>();

        allCategories.Should().Contain(LicenseCategory.AM);
        allCategories.Should().Contain(LicenseCategory.A1);
        allCategories.Should().Contain(LicenseCategory.A2);
        allCategories.Should().Contain(LicenseCategory.A);
        allCategories.Should().Contain(LicenseCategory.B1);
        allCategories.Should().Contain(LicenseCategory.B);
        allCategories.Should().Contain(LicenseCategory.B_E);
        allCategories.Should().Contain(LicenseCategory.C1);
        allCategories.Should().Contain(LicenseCategory.C);
        allCategories.Should().Contain(LicenseCategory.C1_E);
        allCategories.Should().Contain(LicenseCategory.C_E);
        allCategories.Should().Contain(LicenseCategory.D1);
        allCategories.Should().Contain(LicenseCategory.D);
        allCategories.Should().Contain(LicenseCategory.D1_E);
        allCategories.Should().Contain(LicenseCategory.D_E);
        allCategories.Should().Contain(LicenseCategory.T);
        allCategories.Should().Contain(LicenseCategory.PT);
    }

    [Theory]
    [InlineData(LicenseCategory.B)]
    [InlineData(LicenseCategory.A)]
    [InlineData(LicenseCategory.C)]
    public void LicenseCategory_ToString_ShouldReturnCategoryName(LicenseCategory category)
    {
        var result = category.ToString();

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void LicenseCategory_Parse_ShouldConvertFromString()
    {
        var categoryString = "B";

        var category = Enum.Parse<LicenseCategory>(categoryString);

        category.Should().Be(LicenseCategory.B);
    }

    [Fact]
    public void LicenseCategory_TryParse_InvalidCategory_ShouldReturnFalse()
    {
        var categoryString = "InvalidCategory";

        var success = Enum.TryParse<LicenseCategory>(categoryString, out var category);

        success.Should().BeFalse();
    }
}

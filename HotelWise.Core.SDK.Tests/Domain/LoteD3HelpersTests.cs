using System.ComponentModel;
using System.Security.Claims;
using HotelWise.Core.SDK.Extensions;
using HotelWise.Core.SDK.Helpers;
using HotelWise.Core.SDK.Security;
#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.Validation;
using HotelWise.Core.SDK.Common;
#endif

namespace HotelWise.Core.SDK.Tests.Domain;

public class LoteD3HelpersTests
{
    private enum SampleStatus
    {
        [Description("Ativo")]
        Active,
        Inactive
    }

    [Fact]
    public void DataHelper_ConvertSecondsToTimeString_Should_Format()
    {
        DataHelper.ConvertSecondsToTimeString(3661).Should().Be("01:01:01");
    }

    [Fact]
    public void TimeFormatter_Should_Format_Elapsed()
    {
        TimeFormatter.FormatElapsedTime(new TimeSpan(1, 2, 3)).Should().Be("01:02:03");
    }

    [Fact]
    public void EnumExtensions_Should_Read_Description()
    {
        SampleStatus.Active.GetDescription().Should().Be("Ativo");
        SampleStatus.Inactive.GetDescription().Should().Be("Inactive");
    }

    [Fact]
    public void SecurityHelperApi_Should_Parse_UserId()
    {
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "42")]);
        var principal = new ClaimsPrincipal(identity);

        SecurityHelperApi.GetUserIdApi(principal).Should().Be(42);
    }

    [Fact]
    public void MarkdownHelper_Should_Detect_And_Strip()
    {
        MarkdownHelper.HasMarkdown("**bold**").Should().BeTrue();
        MarkdownHelper.RemoveMarkdown("**bold**").Should().Be("bold");
    }

    [Fact]
    public void HtmlHelper_Should_Strip_Tags()
    {
        HtmlHelper.RemoveHtml("<p>Hello</p>").Should().Be("Hello");
    }

    [Fact]
    public void SecurityHelper_Should_Hash_And_Verify_Password()
    {
        SecurityHelper.CreatePasswordHash("secret", out var hash, out var salt);

        hash.Should().NotBeEmpty();
        salt.Should().NotBeEmpty();
        SecurityHelper.VerifyPasswordHash("secret", hash, salt).Should().BeTrue();
        SecurityHelper.VerifyPasswordHash("wrong", hash, salt).Should().BeFalse();
    }

    [Fact]
    public void HelperValidation_TranslateErroCode_Should_Replace_MaxLength()
    {
        HelperValidation.TranslateErroCode("max [MaxLength]", "[10]")
            .Should().Be("max 10");
    }
}

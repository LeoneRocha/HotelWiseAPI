namespace HotelWise.Core.SDK.Tests.Scaffold;

/// <summary>
/// Smoke tests da Fase 0 — validam que o assembly do Core.SDK carrega corretamente.
/// </summary>
public class Phase0ScaffoldTests
{
    [Fact]
    public void CoreSdk_Assembly_Should_Load()
    {
        var assembly = typeof(CoreSdkInfo).Assembly;

        assembly.Should().NotBeNull();
        assembly.GetName().Name.Should().Be("HotelWise.Core.SDK");
    }

    [Fact]
    public void CoreSdkInfo_Should_Expose_PackageMetadata()
    {
        CoreSdkInfo.PackageId.Should().Be(SmartCoreHub.Core.SDK.Common.CoreSdkInfo.PackageId);
        CoreSdkInfo.Version.Should().Be(SmartCoreHub.Core.SDK.Common.CoreSdkInfo.Version);
    }
}

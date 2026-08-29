using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelWise.Data.Context.Configure.Helper
{
    /// <summary>
    /// ⚠️ Charset canônico no Core; aplicação Pomelo HasCharSet permanece no host.
    /// </summary>
    [Obsolete(
        "Charset DefaultCharSet movido para HotelWise.Core.SDK.Infrastructure.HelperCharSet. AddCharSet (Pomelo) permanece no host.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_REPO")]
    public static class HelperCharSet
    {
        public const string DefaultCharSet = HotelWise.Core.SDK.Infrastructure.HelperCharSet.DefaultCharSet;

        public static void AddCharSet<T>(EntityTypeBuilder<T> builder) where T : class
        {
            builder.HasCharSet(DefaultCharSet);
        }
    }
}

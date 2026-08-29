using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelWise.Data.Context.Configure.Helper
{
    /// <summary>
    /// Aplicação Pomelo <c>HasCharSet</c> no host.
    /// Charset canônico: <see cref="HotelWise.Core.SDK.Infrastructure.HelperCharSet.DefaultCharSet"/>.
    /// </summary>
    public static class PomeloCharSetHelper
    {
        public static void AddCharSet<T>(EntityTypeBuilder<T> builder) where T : class
        {
            builder.HasCharSet(HotelWise.Core.SDK.Infrastructure.HelperCharSet.DefaultCharSet);
        }
    }
}

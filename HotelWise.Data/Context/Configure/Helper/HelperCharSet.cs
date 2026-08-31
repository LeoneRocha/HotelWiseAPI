using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelWise.Data.Context.Configure.Helper;

/// <summary>
/// Classe auxiliar para aplicação do CharSet padrão do Pomelo MySQL nas entidades de banco.
/// </summary>
public static class PomeloCharSetHelper
{
    /// <summary>
    /// Aplica o charset padrão canônico (<see cref="SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.Ported.HelperCharSet.DefaultCharSet"/>) na tabela da entidade.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade persistida.</typeparam>
    /// <param name="builder">Construtor de tipo de entidade.</param>
    public static void AddCharSet<T>(EntityTypeBuilder<T> builder) where T : class
    {
        builder.HasCharSet(SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.Ported.HelperCharSet.DefaultCharSet);
    }
}

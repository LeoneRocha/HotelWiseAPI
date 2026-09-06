using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchHelperCharSet = SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.HelperCharSet;

namespace HotelWise.Data.Context.Configure.Helper;

/// <summary>
/// Classe auxiliar para aplicação do CharSet padrão do Pomelo MySQL nas entidades de banco.
/// Mantém latin1 (schema legado HW) via canônico <see cref="SchHelperCharSet.Latin1"/>.
/// </summary>
public static class PomeloCharSetHelper
{
    /// <summary>
    /// Aplica o charset latin1 canônico na tabela da entidade (sem migrar para utf8mb4).
    /// </summary>
    /// <typeparam name="T">Tipo da entidade persistida.</typeparam>
    /// <param name="builder">Construtor de tipo de entidade.</param>
    public static void AddCharSet<T>(EntityTypeBuilder<T> builder) where T : class
    {
        builder.HasCharSet(SchHelperCharSet.Latin1);
    }
}

using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Data.Context.Configure.Mock;

/// <summary>
/// Fornece dados iniciais (seed/mock) de hotéis para inicialização de banco e testes.
/// </summary>
public static class HotelsMockData
{
    private static readonly string[] _tags = { "Luxury", "Spa" };
    private static readonly DateTime SeedCreatedDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(3290);
    private static readonly DateTime SeedModifyDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(3292);

    /// <summary>
    /// Retorna uma lista de hotéis de exemplo para popular o banco de dados via seed.
    /// </summary>
    /// <returns>Array de entidades <see cref="Hotel"/> para carga inicial.</returns>
    public static Hotel[] GetHotels()
    {
        return
        [
            new Hotel
            {
                HotelId = 1,
                HotelName = "Hotel Example",
                Description = "An example hotel",
                Tags = _tags,
                Stars = 5,
                InitialRoomPrice = 812.006730389325700m,
                ZipCode = "55093-317",
                Location = "Travessa 5491 Carvalho Travessa",
                City = "São Vicente",
                StateCode = "SP",
                CreatedUserId = 1,
                ModifyUserId = 1,
                CreatedDate = SeedCreatedDate,
                ModifyDate = SeedModifyDate
            }
        ];
    }
}

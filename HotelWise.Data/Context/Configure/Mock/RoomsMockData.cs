using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Data.Context.Configure.Mock;

/// <summary>
/// Fornece dados iniciais (seed/mock) de quartos para inicialização de banco de dados e testes.
/// </summary>
public static class RoomsMockData
{
    private static readonly DateTime SeedCreatedDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(5000);
    private static readonly DateTime SeedModifyDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(5001);

    /// <summary>
    /// Retorna quartos pré-configurados para a carga de seed do Entity Framework Core.
    /// </summary>
    /// <returns>Array de entidades <see cref="Room"/> para carga inicial.</returns>
    public static Room[] GetRooms()
    {
        return
        [
            new Room
            {
                Id = 100,
                HotelId = 1,
                Name = "Quarto Example",
                Description = "Quarto de exemplo seed pós .NET 10",
                RoomType = RoomType.Double,
                Capacity = 2,
                Status = RoomStatus.Available,
                MinimumNights = 1,
                CreatedUserId = 1,
                ModifyUserId = 1,
                CreatedDate = SeedCreatedDate,
                ModifyDate = SeedModifyDate
            }
        ];
    }
}

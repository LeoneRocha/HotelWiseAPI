using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Context;

/// <summary>
/// Contexto do Entity Framework Core para o banco de dados MySQL da aplicação HotelWise.
/// Gerencia os conjuntos de entidades de hotéis, quartos, reservas, usuários e histórico de chat IA.
/// </summary>
public class HotelWiseDbContextMysql : DbContext
{
    /// <summary>
    /// Conjunto de dados da entidade <see cref="Hotel"/>.
    /// </summary>
    public virtual DbSet<Hotel> Hotels { get; set; }

    /// <summary>
    /// Conjunto de dados da entidade <see cref="User"/>.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// Conjunto de dados de histórico de conversas <see cref="ChatSessionHistory"/>.
    /// </summary>
    public virtual DbSet<ChatSessionHistory> ChatSessionHistories { get; set; }

    /// <summary>
    /// Conjunto de dados da entidade de quartos <see cref="Room"/>.
    /// </summary>
    public virtual DbSet<Room> Rooms { get; set; }

    /// <summary>
    /// Conjunto de dados de disponibilidade e precificação <see cref="RoomAvailability"/>.
    /// </summary>
    public virtual DbSet<RoomAvailability> RoomAvailabilities { get; set; }

    /// <summary>
    /// Conjunto de dados das reservas <see cref="Reservation"/>.
    /// </summary>
    public virtual DbSet<Reservation> Reservations { get; set; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="HotelWiseDbContextMysql"/> com as opções de conexão configuradas.
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public HotelWiseDbContextMysql(DbContextOptions<HotelWiseDbContextMysql> options) : base(options) { }

    /// <summary>
    /// Configura o modelo relacional e os mapeamentos Fluent API das entidades.
    /// </summary>
    /// <param name="modelBuilder">Construtor de modelos do EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configure FLUENT API 
        ConfigurationEntitiesHelper.AddConfigurationEntitiesManually(modelBuilder);
        ConfigurationEntitiesHelper.AddConfigurationEntities(modelBuilder);
    }
}

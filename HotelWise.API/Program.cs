using HotelWise.API.Configure;

namespace HotelWise.API;

/// <summary>
/// Ponto de entrada principal (Entry Point) da aplicação Web API do HotelWise.
/// </summary>
public static class Program
{
    /// <summary>
    /// Método de inicialização que constrói o host e executa o servidor web ASP.NET Core.
    /// </summary>
    /// <param name="args">Argumentos da linha de comando passados na execução.</param>
    public static void Main(string[] args)
    {
        try
        {
            var hostBuilder = WebApplicationConfigureBuilder.CreateHostBuilder(args);
            WebApplicationConfigureBuilder.BuildAndRunAPP(hostBuilder.Item1, hostBuilder.Item2);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("FATAL: HotelWise.API failed to start.");
            Console.Error.WriteLine(ex.ToString());
            throw;
        }
    }
}

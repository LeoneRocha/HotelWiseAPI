using HotelWise.API.Configure;

public static class Program
{
    public static void Main(string[] args)
    {
        //var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        //builder.Services.AddControllers();
        //var app = builder.Build();

        //// Configure the HTTP request pipeline.

        //app.UseHttpsRedirection();

        //app.UseAuthorization();

        //app.MapControllers();

        //app.Run(); 

        var hostBuilder = WebApplicationConfigureBuilder.CreateHostBuilder(args);
         
        WebApplicationConfigureBuilder.BuildAndRunAPP(hostBuilder.Item1, hostBuilder.Item2); 
    }
}
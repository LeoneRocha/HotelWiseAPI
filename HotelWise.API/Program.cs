using Google.Protobuf.WellKnownTypes;
using HotelWise.API.Configure;

public static class Program
{
    public static void Main(string[] args)
    { 

        var hostBuilder = WebApplicationConfigureBuilder.CreateHostBuilder(args);
         
        WebApplicationConfigureBuilder.BuildAndRunAPP(hostBuilder.Item1, hostBuilder.Item2); 
    }
}
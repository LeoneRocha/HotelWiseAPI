using HotelWise.API.Configure;

namespace HotelWise.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            var hostBuilder = WebApplicationConfigureBuilder.CreateHostBuilder(args);

            WebApplicationConfigureBuilder.BuildAndRunAPP(hostBuilder.Item1, hostBuilder.Item2);
        }
    }
}
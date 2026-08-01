using HotelWise.API.Configure;

namespace HotelWise.API
{
    public static class Program
    {
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
}

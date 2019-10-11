using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WR.Modelo.ApiMs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
                                                                     .UseUrls("http://*:5500")
                                                                     .UseStartup<Startup>()
                                                                     .Build();
    }
}

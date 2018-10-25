using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XGTech.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             //.UseKestrel(options =>
             //{
             //    options.Listen(IPAddress.Any, 1818);
             //})
                .UseStartup<Startup>()
                .Build();


        //WebHost.CreateDefaultBuilder(args)
        //         .UseKestrel(options =>
        //         {
        //    options.Listen(IPAddress.Any, 5000);
        //    options.Listen(IPAddress.Any, 7777, listenOptions =>
        //    {
        //        listenOptions.UseHttps("server.pfx", "linezero");
        //    });
        //})
        //         .ConfigureLogging((hostingContext, logging) =>
        //         {
        //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        //    logging.AddConsole();
        //})
        //        .UseStartup<Startup>()
        //        .Build();

    }
}

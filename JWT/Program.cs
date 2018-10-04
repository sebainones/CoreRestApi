using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JWT
{
    public class Program
    {
        private static IConfigurationRoot config;

        public static void Main(string[] args)
        {
            config = BuildConfig("appsettings");
            CreateWebHostBuilder(args)                
                //.Configure(a => a.Run(c => c.Response.WriteAsync("Hello Asp.Net Core World!")))
                .Build()
                .Run();
        }

        private static IConfigurationRoot BuildConfig(string jsonFileName)
        {
            return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddEnvironmentVariables()
                        .AddJsonFile($"jsonFileName{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)                        
                        .Build();
        }

        // Remarks:
        //     The following defaults are applied to the returned Microsoft.AspNetCore.Hosting.WebHostBuilder:
        //     - Use Kestrel as the web server and configure it using the application's configuration providers,
        //     - Set the Microsoft.AspNetCore.Hosting.IHostingEnvironment.ContentRootPath  to the result of System.IO.Directory.GetCurrentDirectory, 
        //     - Load Microsoft.Extensions.Configuration.IConfiguration  from 'appsettings.json' and 'appsettings.[Microsoft.AspNetCore.Hosting.IHostingEnvironment.EnvironmentName].json',
        //     - Load Microsoft.Extensions.Configuration.IConfiguration from User Secrets when  Microsoft.AspNetCore.Hosting.IHostingEnvironment.EnvironmentName is 'Development'  using the entry assembly,
        //     - Load Microsoft.Extensions.Configuration.IConfiguration  from environment variables, 
        //     - Configures the Microsoft.Extensions.Logging.ILoggerFactory to log to the console and debug output,
        //     - Enables IIS integration, and enables
        //     the ability for frameworks to bind their options to their default configuration
        //     sections.
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                //.ConfigureLogging() //configureloggin mechanism for startup
            ;
    }
}

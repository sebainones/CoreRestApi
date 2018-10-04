using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JWT.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag.AspNetCore;

namespace JWT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. 
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add application Services
            //...

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("CountryDB"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(ConfigureJson);

            //For REST Api Documentation
            services.AddSwaggerGen(c => c.SwaggerDoc("V1", new Swashbuckle.AspNetCore.Swagger.Info() { Title = "Docum" }));
        }

        private void ConfigureJson(MvcJsonOptions jsonOptions)
        {
            jsonOptions.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        // Configure and arrange Middleware! 
        //The order in which appear sentences in here is very important!
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(LogLevel.Trace);
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();??

                if (context != null && !context.Countries.Any())
                {
                    context.Countries.AddRange(new Country { Id = 1, Name = "Argentina", States = new List<State>() { new State { Id = 1, Name = "Mendoza" } } }, new Country { Id = 2, Name = "Chile" });
                    context.SaveChanges();

                }
            }
            else
            {
                app.UseHsts();
            }

            //For REST Api Documentation
            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.1
            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml

            //https://localhost:44375/swagger/
            // Enable the Swagger UI middleware and the Swagger generator
            app.UseSwaggerUi3WithApiExplorer(settings => settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase);

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}

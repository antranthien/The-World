using AutoMapper;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using TheWorld2.Models;
using TheWorld2.Services;
using TheWorld2.ViewModels;

namespace TheWorld2
{
    public class Startup
    {
        // make this static so it can be accessed staticly
        public static IConfigurationRoot Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public Startup(IApplicationEnvironment appEnv)
        {
            // Read the config.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath) // the builder will know where to locate the config.json file
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(opt => { // Config JSON format
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            // Add Logging
            services.AddLogging();

            // EF declaration
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<WorldContext>();

            // DI declaration
            services.AddScoped<IMailService, DebugMailService>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddTransient<WorldContextSeedData>();
            services.AddScoped<CoordinateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory logger)
        {
            // if deploying in IIS, use this
            //app.UseIISPlatformHandler();

            // search for a index.html file in root folder by default
            //app.UseDefaultFiles();

            logger.AddDebug(LogLevel.Warning);

            // We want to use a static HTML file by default
            app.UseStaticFiles();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
            });

            app.UseMvc(config => // config the route
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new {controller = "App", action = "Index"}
                );
            });

            seeder.SeedData();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}

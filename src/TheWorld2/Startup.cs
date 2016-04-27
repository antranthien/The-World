using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using TheWorld2.Models;
using TheWorld2.Services;

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
            services.AddMvc();
            // EF declaration
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<WorldContext>();

            services.AddScoped<IMailService, DebugMailService>(); // DI declaration
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // if deploying in IIS, use this
            //app.UseIISPlatformHandler();

            // search for a index.html file in root folder by default
            //app.UseDefaultFiles();


            // We want to use a static HTML file by default
            app.UseStaticFiles();

            app.UseMvc(config => // config the route
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new {controller = "App", action = "Index"}
                );
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}

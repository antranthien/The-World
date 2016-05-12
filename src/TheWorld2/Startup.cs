using AutoMapper;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Threading.Tasks;
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
            //services.AddMvc(config =>
            //{
            //    #if !DEBUG
            //        config.Filters.Add(new RequireHttpsAttribute());
            //    #endif
            //})
            //.AddJsonOptions(opt => { // Config JSON format
            //    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //});

            services.AddMvc()
            .AddJsonOptions(opt => { // Config JSON format
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // Identity
            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";

                // Events are callbacks that Identity allows us to override to change default behaviors
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    // lambda supports async
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }                 
                        else
                        {
                            //this is the default behavior
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.FromResult(0);
                    }
                };
            })
            .AddEntityFrameworkStores<WorldContext>();

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
        public async void Configure(IApplicationBuilder app, 
            WorldContextSeedData seeder, 
            ILoggerFactory logger,
            IHostingEnvironment env)
        {
            // if deploying in IIS, use this
            //app.UseIISPlatformHandler();

            // search for a index.html file in root folder by default
            //app.UseDefaultFiles();

            if (env.IsDevelopment())
            {
                logger.AddDebug(LogLevel.Information);
                // add middleware
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.AddDebug(LogLevel.Error);
                app.UseExceptionHandler("/App/Error");
            }
           

            // We want to use a static HTML file by default
            app.UseStaticFiles();

            // Use Identity
            app.UseIdentity();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
            });

            // Mvc is one of the last items to be included
            app.UseMvc(config => // config the route
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                );
            });

            await seeder.SeedData();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}

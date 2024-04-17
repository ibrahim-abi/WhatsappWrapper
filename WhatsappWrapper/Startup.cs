using LoggerService;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using System.Reflection;
using WhatsappWrapper.Controllers;
using WhatsappWrapper.Extentions;
using WhatsappWrapper.Processor;
using IApplicationLifetime = Microsoft.Extensions.Hosting.IApplicationLifetime;

namespace WhatsappWrapper
{
    public class Startup
    {
        public static IHttpClientFactory httpClientFactory { get; }
        public static IDictionary<string, string> ServiceURLS { get; set; }
        public static IConfiguration configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddEndpointsApiExplorer();
            services.AddControllers();
            services.AddSingleton<ChromeDriver>(sp => DriverConfiguration.GetDriver());
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddHostedService<WebDriverDisposeService>();
            services.ConfigurationCors();
            services.ConfigureIISIntegration();
            services.AddHealthChecks();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            ServiceURLS = Configuration.GetSection("Appsettingkeys").Get<Dictionary<string, string>>();
            services.AddHttpClient();
            services.AddHealthChecks();
            services.AddTransient<Health>();

        }

        public void Configure(IApplicationBuilder app,IApplicationLifetime appLifetime)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
                options.RoutePrefix = string.Empty; 
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseRouting();
            appLifetime.ApplicationStopping.Register(() =>
            {
                    DriverConfiguration.Dispose(); // Dispose of the ChromeDriver instance
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHealthChecks("/health", new HealthCheckOptions
            //    {
            //        ResponseWriter = async (context, report) =>
            //        {
            //            var result = JsonConvert.SerializeObject(
            //                new
            //                {
            //                    status = report.Status.ToString(),
            //                    checks = report.Entries.Select(entry => new
            //                    {
            //                        name = entry.Key,
            //                        status = entry.Value.Status.ToString(),
            //                        description = entry.Value.Description
            //                    })
            //                });
            //            context.Response.ContentType = "application/json";
            //            await context.Response.WriteAsync(result);
            //        }
            //    });
            //});
        }
    }
}

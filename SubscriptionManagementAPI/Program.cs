
using Serilog;
using SubscriptionManagement.API.Extensions;
using SubscriptionManagement.Application.Helpers;
using SubscriptionManagement.Application.Utilities;

namespace SubscriptionManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var config = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false)
      .Build();
            var endpoint = new AppSettings();
            config.GetSection("AppSettings").Bind(endpoint);
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.SqlServer", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                .WriteTo.File(
                 $@"{endpoint.LogPath}\SubscriptionManagement_API_{DateTime.Now:ddMMyyyy}.txt",
             fileSizeLimitBytes: 15_000_000,
             rollOnFileSizeLimit: true,
             shared: true,
             flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();

            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddJwt(builder.Configuration);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            bool prod = !string.IsNullOrEmpty(endpoint.Swagger) && endpoint.Swagger.ToLower().StartsWith("n");
            if (!prod)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHsts();
            }

            app.UseExceptionHandleMiddleware();

            app.UseHttpsRedirection();
            app.UseCors(x =>
            {
                x.WithOrigins(builder.Configuration["AllowedHosts"]
                  .Split(",", StringSplitOptions.RemoveEmptyEntries)
                  .Select(o => o.RemovePostFix("/"))
                  .ToArray())
             .AllowAnyMethod()
             .AllowAnyHeader();
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SubscriptionManagement.Application.Helpers;
using SubscriptionManagement.Application.Interfaces;
using SubscriptionManagement.Application.Models.Request;
using SubscriptionManagement.Application.Repositories;
using SubscriptionManagement.Application.Services;
using SubscriptionManagement.Application.Utilities;
using SubscriptionManagement.Application.Validations;
using SubscriptionManagement.Infrastructure.DataContext;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SubscriptionManagement.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static readonly ILoggerFactory dbLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<SubscriptionManagementDataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            SubscriptionManagementDataContext.Connectionstring = config.GetConnectionString("DefaultConnection");
            // Other service configurations...

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 1073741824; // 1 GB in bytes
            });
            services.AddSingleton<HtmlEncoder>(
    HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                               UnicodeRanges.CjkUnifiedIdeographs }));
            // Other service configurations...

            services.AddScoped<HttpClient>();


            services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IServiceRepo, ServiceRepo>();
            services.AddScoped<ISubscriberRepo, SubscriberRepo>();


            services.AddScoped<IService, Services>();


            //services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            //services.AddScoped<EncryptionActionFilter>();
            services.AddHttpContextAccessor();

            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<SubscribeRequest>, SubscribeRequestValidator>();
            return services;
        }

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(Convert.ToDouble(configuration["HttpSecurity:expiryTime"]));
                options.ExcludedHosts.Add(configuration["HttpSecurity:Url"]);
                options.ExcludedHosts.Add(configuration["HttpSecurity:Url"]);
            });
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<JwtHandler>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Subscription Management API Services", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
        }
    }
}

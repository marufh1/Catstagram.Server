
namespace Catstagram.Server.Infrastructure.Extensions
{
    using Catstagram.Server.Data;
    using Catstagram.Server.Data.Model;
    using Catstagram.Server.Features.Cats;
    using Catstagram.Server.Features.Identity;
    using Catstagram.Server.Infrastructure.Filters;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using System.Text;

    public static class ServiceCollectionExtensions
    {
        public static ApplicationSettings GetApplicationSettings(
           this IServiceCollection services,
           IConfiguration configuration
           )
        {
            var applicationSettingsConfiguration = configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(applicationSettingsConfiguration);

            return applicationSettingsConfiguration.Get<ApplicationSettings>();
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services
            , IConfiguration configuration)
            => services.AddDbContext<CatstagramDbContext>(options =>
                options.UseSqlServer(configuration.GetDefaultConnecionString()));

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 5;
                })
                .AddEntityFrameworkStores<CatstagramDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            ApplicationSettings appSettings)
        {
            var key = Encoding.UTF8.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   //ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   //ValidIssuer = Configuration["Jwt:Issuer"],
                   //ValidAudience = Configuration["Jwt:Issuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(key)
               };
           });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            => services
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<ICatService, CatService>();

        public static IServiceCollection AddSwagger(this IServiceCollection services) 
            => services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", 
                    new OpenApiInfo { 
                        Title = "My Cats API" 
                    });
            });

        public static void AddApiControllers(this IServiceCollection services)
            => services.AddControllers(options => options
                            .Filters
                            .Add<ModelOrNotFoundActionFilter>());
    }
}

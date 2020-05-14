using Microsoft.Extensions.DependencyInjection;
using CentralErros.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CentralErros.Models;
using CentralErros.Utils;
namespace CentralErros.ConfigStartup
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connection = Utils.Utils.DecryptConnectionString(configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // JWT
            // acesso � se��o do arq appsetiings
            var appSettingsSection = configuration.GetSection("AppSettings");
            
            // config com parametros
            services.Configure<AppSettings>(appSettingsSection);

            // cast para objeto
            var appSettings = appSettingsSection.Get<AppSettings>();
            
            // chave gerada a partir do parametro
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            // config autentica��o e Bearer
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });

            return services;
        }
    }
}

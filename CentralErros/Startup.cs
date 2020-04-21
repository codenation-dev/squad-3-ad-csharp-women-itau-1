using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using CentralErros.Api.Models;
using CentralErros.ConfigStartup;
using CentralErros.Filters;
using CentralErros.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CentralErros
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public StartupIdentityServer IdentitServerStartup { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;

            //config ambiente se não for teste
            if (!environment.IsEnvironment("Testing"))
                IdentitServerStartup = new StartupIdentityServer(environment);
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //add politica para user Ingrid
            services.AddMvcCore()
               .AddAuthorization(opt =>
               {
                   opt.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Email, "jaquelinelaurenti@gmail.com"));
               })
               .AddJsonFormatters()
               .AddApiExplorer()
               .AddVersionedApiExplorer(p =>
               {
                   p.GroupNameFormat = "'v'VVV";
                   p.SubstituteApiVersionInUrl = true;
               });

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ErrorResponseFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<CentralErroContexto>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEnvironmentService, EnvironmentService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // config prop IdentitServerStartup
            if (IdentitServerStartup != null)
                IdentitServerStartup.ConfigureServices(services);

            // config versionamento
            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // config swagger para gerar arquivo de documentação swagger.json
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityRequirement(
                    new Dictionary<string, IEnumerable<string>> {
                            { "Bearer", new string[] { } }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}"
                });

            });


            // config autenticação para API - jwt bearer 
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5001";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "codenation";
                });

            // config desab validação de Model State automatico
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (IdentitServerStartup != null)
                IdentitServerStartup.Configure(app, env);

            // swagger
            app.UseSwagger();

            // swagger UI
            app.UseSwaggerUI(options =>
            {
                //s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
                }

                options.DocExpansion(DocExpansion.List);
            });

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}

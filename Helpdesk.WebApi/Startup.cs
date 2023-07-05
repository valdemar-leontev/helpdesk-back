using System.Reflection;
using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.WebApi.Config;
using Helpdesk.WebApi.Middlewares;
using Helpdesk.WebApi.Services;
using Helpdesk.WebApi.Services.Mapper;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Helpdesk.WebApi;

public class Startup
{
    public readonly ApplicationConfig ApplicationConfig = new();

    public IConfiguration Configuration { get; }

    private readonly IWebHostEnvironment _webHostEnvironment;

    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        configuration.Bind(ApplicationConfig);
        _webHostEnvironment = webHostEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    // options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = ApplicationConfig.AuthenticationConfig!.Issuer,
                        ValidateAudience = true,
                        ValidAudience = ApplicationConfig.AuthenticationConfig.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = ApplicationConfig.AuthenticationConfig.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                })
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, _ => { });

        services.AddHttpContextAccessor();

        services.AddDataProtection();

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyAuthenticationOptions.DefaultScheme)
                .Build();
        });

        services.AddControllersWithViews();

        services.AddCors();

        services.AddDbContext<AppDatabaseContext>(options =>
        {
            var defaultConnectionString = Configuration.GetConnectionString("DefaultConnection");
            var dockerConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            options.UseNpgsql(dockerConnectionString ?? defaultConnectionString,
                postgresqlOptions => { postgresqlOptions.MigrationsAssembly("Helpdesk.WebApi"); }).UseSnakeCaseNamingConvention();
        });

        services
            .AddControllers()
            .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

        services.AddSingleton<IApplicationConfig>(ApplicationConfig);

        services.AddHttpContextAccessor();

        services.AddServices();

        services.AddAutoMapper(options =>
        {
            var profiles = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t))
                .Except(new[] { typeof(FileMapperProfile) })
                .Select(Activator.CreateInstance)
                .Union(new[]
                {
                    new FileMapperProfile(_webHostEnvironment)
                })
                .Cast<Profile>();

            options.AddProfiles(profiles);
        });

        services.AddSwaggerGen(options => { options.SwaggerDoc("v1", new OpenApiInfo { Title = "Helpdesk.WebApi", Version = "v1" }); });
    }

    public void Configure(IApplicationBuilder app, AppDatabaseContext dataContext)
    {
        if (_webHostEnvironment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        dataContext.Database.EnsureDeleted();
        dataContext.Database.Migrate();

        app.UseMiddleware<ExceptionMiddleware>();

        app
            .UseStaticFiles()
            .UseSwagger(options => { options.RouteTemplate = "docs/api/{documentname}/schema.json"; })
            .UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs/api";
                options.DocumentTitle = "Helpdesk API Console";
                options.InjectStylesheet("/content/css/swagger-custom.css");
                options.InjectJavascript("/content/js/swagger-custom.js");
                options.SwaggerEndpoint("/docs/api/v1/schema.json", "Helpdesk.WebApi v1");
            });

        app.UseCors(options =>
        {
            options.AllowAnyMethod();
            options.AllowAnyHeader();
            options.AllowCredentials();

            options.WithOrigins(
                "http://localhost",
                "http://localhost:3000",
                "http://localhost:3001",
                "http://localhost:3002",
                "http://localhost:5004"
            ).WithExposedHeaders(
                "Content-Disposition",
                "X-Suggested-Filename"
            );
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapControllerRoute("Default", "{controller=Default}/{action=Index}");
        });
    }
}
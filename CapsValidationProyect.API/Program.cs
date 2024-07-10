using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CapsValidationProyect.API.Middleware;
using CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;
using CapsValidationProyect.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCustomConfiguration(builder.Configuration)
    .AddCustomMvc()
    .AddHealthChecks(builder.Configuration)
    .AddCustomDbContext(builder.Configuration)
    .AddCustomSwagger(builder.Configuration)
    .AddCustomAuthentication(builder.Configuration)
    .AddCustomAuthorization(builder.Configuration)
    .AddCustomIC(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapControllers();
app.UseMiddleware<HandlerExceptionMiddleware>();

#region Database Migration && ContextSeed Inisializtion
try
{
    Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CapsTestContext>();
    var env = app.Services.GetService<IWebHostEnvironment>();
    var settings = app.Services.GetRequiredService<IOptions<AppSettings>>();
    //var logger = app.Services.GetService<ILogger<AppContextSeed>>();
    //var userManager = (UserManager<EmployeeUser>)scope.ServiceProvider.GetService(typeof(UserManager<EmployeeUser>));
    await context.Database.MigrateAsync();
    //await new AppContextSeed().SeedAsync(context, env, settings, logger, userManager);

    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    await app.RunAsync();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
#endregion

public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}

static class CustomExtensionsMethods
{

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CurrentUser.Handler).Assembly));

        // Add framework services.
        services.AddControllers(opt =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            opt.Filters.Add(new AuthorizeFilter(policy));
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });



        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CapsTestContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(CapsTestContext).Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
        },
        ServiceLifetime.Scoped
           );
        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ideas Commscope - Core HTTP API",
                Version = "v1",
                Description = "The Ideas Commscope Microservice HTTP API."
            });
            //options.CustomSchemaIds(c => c.FullName);
        });

        return services;
    }


    public static IServiceCollection AddCustomIC(this IServiceCollection services, IConfiguration configuration)
    {
        #region Repositories
        //services.AddScoped<IBlogDocumentRepository, BlogDocumentRepository>();
        #endregion

        #region Servces
        ///
        //services.AddAutoMapper(typeof(Query.Handler));

        ///Servces
        ///
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddHttpContextAccessor();

        //Security
        //services.AddScoped<IJwtGenerator, JwtGenerator>();
        //services.AddScoped<IUserSerssion, UserSession>();
        services.AddScoped<UserManager<EmployeeUser>>();

        // Register TimeProvider
        services.AddSingleton<TimeProvider>(TimeProvider.System);

        //services.AddAutoMapper(typeof(CurrentUser.Handler));

        #endregion

        return services;
    }



    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
    {


        services.AddOptions();
        services.Configure<AppSettings>(configuration);
        services.Configure<AppSettings>(options =>
        {
            options.ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        });
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });

        services.AddSerilog();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                .WriteTo.File("Logs/Error/log-.txt",
                    rollingInterval: RollingInterval.Day, //Intervalo diario
                    retainedFileCountLimit: 20, //limite de archivos, se reemplaza en ultimo
                    fileSizeLimitBytes: 10485760)) // 10 MB 
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                .WriteTo.File("Logs/Info/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 20,
                    fileSizeLimitBytes: 10485760))
            .CreateLogger();

        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue; // Tamaño límite de los datos enviados en megabytes
            options.MultipartBodyLengthLimit = long.MaxValue; // Tamaño límite de todo el cuerpo de la solicitud en bytes
            options.MemoryBufferThreshold = int.MaxValue;
        });

        return services;
    }



    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi@#palabra234234asdas1212312423654636sdfaszf1242345sfsdfsdfsdfsecreta"));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateAudience = false,
                ValidateIssuer = false
            };
        });


        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = services.AddIdentityCore<EmployeeUser>();
        var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

        identityBuilder.AddRoles<IdentityRole<int>>();
        identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<EmployeeUser, IdentityRole<int>>>();

        identityBuilder.AddEntityFrameworkStores<CapsTestContext>();
        identityBuilder.AddSignInManager<SignInManager<EmployeeUser>>();

        // Register default services for Identity
        services.AddScoped<ISecurityStampValidator, SecurityStampValidator<EmployeeUser>>();
        services.AddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<EmployeeUser>>();
        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}



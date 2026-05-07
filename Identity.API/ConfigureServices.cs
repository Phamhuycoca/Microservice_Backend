using Identity.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using SharedApi;
using SharedApplication;
using SharedInfrastructure;
using Identity.Application;

namespace Identity.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApicontrollerServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddHttpContextAccessor();
        //Cấu hình config BaseController
        services.AddApiBaseControllerCQRSServices(configuration, typeof(Program).Assembly, jwtSectionName: "JwtSettings", titleApp: "Identity Servcie");
        //Cấu hình config application
        services.AddApplicationCQRSServices(typeof(Program).Assembly);
        services.AddInfrastructureCQRSModule<IdentityDbContext>(configuration, options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql =>
                {
                    sql.EnableRetryOnFailure();
                    sql.CommandTimeout(60);
                });
        });
        services.AddApplicationServices();
        return services;
    }
}

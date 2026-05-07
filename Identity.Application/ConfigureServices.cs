using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Identity.Infrastructure;

namespace Identity.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(assembly);
        });
        IMapper mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        services.AddMediatR(ctg =>
        {
            ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddInfrastructureModule();
        return services;
    }
}

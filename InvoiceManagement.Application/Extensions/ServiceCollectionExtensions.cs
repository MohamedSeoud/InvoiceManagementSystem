using AutoMapper;
using InvoiceManagement.Application.Interfaces;
using InvoiceManagement.Application.Services;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IInvoiceService, InvoiceService>();
        return services;
    }
}
namespace AmsApp.Helpers
{
   //using AmsApp.Service;
    using Microsoft.Extensions.DependencyInjection;
    using System.Security.Claims;

    public static class CoreServiceExtensions
    {
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
        {
           
            return services;
        }
    }
}

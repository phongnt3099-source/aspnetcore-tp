using Microsoft.Extensions.DependencyInjection;
using ThienPhucDental.HealthChecks;

namespace ThienPhucDental.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<ThienPhucDentalDbContextHealthCheck>("Database Connection");
            builder.AddCheck<ThienPhucDentalDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ThienPhucDental.EntityFrameworkCore;

namespace ThienPhucDental.HealthChecks
{
    public class ThienPhucDentalDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public ThienPhucDentalDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("ThienPhucDentalDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("ThienPhucDentalDbContext could not connect to database"));
        }
    }
}

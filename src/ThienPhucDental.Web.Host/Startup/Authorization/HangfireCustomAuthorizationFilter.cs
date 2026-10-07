using Hangfire.Dashboard;

namespace ThienPhucDental.Web.Startup.Authorization
{
    public class HangfireCustomAuthorizationFilter: IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var remoteIp = httpContext.Connection.RemoteIpAddress;

            // Cho phép tất cả request (cả GET và POST/Action) trên localhost
            if (remoteIp != null && System.Net.IPAddress.IsLoopback(remoteIp))
                return true;

            if (httpContext.User.Identity?.IsAuthenticated != true)
                return false;

            // Môi trường Production: kiểm tra đăng nhập
            return httpContext.User.IsInRole("Admin");
        }
    }
}

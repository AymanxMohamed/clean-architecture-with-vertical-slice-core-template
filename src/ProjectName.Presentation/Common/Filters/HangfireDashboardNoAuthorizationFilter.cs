using Hangfire.Dashboard;

namespace ProjectName.Presentation.Common.Filters;

public class HangfireDashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
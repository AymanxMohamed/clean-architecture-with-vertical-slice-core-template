using ProjectName.Application.Common.Services;
using ProjectName.Application.Common.Services.BackgroundJobs;

namespace ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.CronExpressions;

public class CronExpressionGenerator : ICronExpressionGenerator
{
    public string MinutesInterval(int n)
    {
        if (n is <= 0 or > 59)
        {
            throw new ArgumentException("Invalid interval. Value must be greater than 0 and less than or equal to 59.");
        }

        return $"*/{n} * * * *";
    }
    
    public string SecondsInterval(int n)
    {
        if (n <= 0 || n > 59)
        {
            throw new ArgumentException("Invalid interval. Value must be greater than 0 and less than or equal to 59.", nameof(n));
        }

        return $"*/{n} * * * * *";
    }
}
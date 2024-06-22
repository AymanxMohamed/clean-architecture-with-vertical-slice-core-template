namespace ProjectName.Application.Common.Services.BackgroundJobs;

public interface ICronExpressionGenerator
{
    string MinutesInterval(int n);

    string SecondsInterval(int n);
}
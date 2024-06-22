using System.Reflection;

namespace ProjectName.Application.Common.Services.BackgroundJobs;

public interface IBackgroundJob
{
    void Run();
}
namespace ProjectName.Domain.Common.Services;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }

    public DateTime Now { get; }
}
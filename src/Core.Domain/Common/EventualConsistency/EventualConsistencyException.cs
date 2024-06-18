namespace Core.Domain.Common.EventualConsistency;

public class EventualConsistencyException(Error eventualConsistencyError, List<Error>? underlyingErrors = null)
    : ApplicationException(message: eventualConsistencyError.Description)
{
    public Error EventualConsistencyError { get; } = eventualConsistencyError;

    public List<Error> UnderlyingErrors { get; } = underlyingErrors ?? [];
}
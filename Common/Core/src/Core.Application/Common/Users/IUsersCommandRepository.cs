using Core.Domain.Aggregates.UserAggregate;

namespace Core.Application.Common.Users;

public interface IUsersCommandRepository
{
    Task CreateAsync(User user);
    
    Task UpdateAsync(User user);
}
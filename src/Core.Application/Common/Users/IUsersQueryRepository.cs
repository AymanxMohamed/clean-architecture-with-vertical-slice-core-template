using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Core.Application.Common.Users;

public interface IUsersQueryRepository
{
    Task<bool> ExistsByEmailAsync(string email);
    
    Task<User?> GetByEmailAsync(string email);
    
    Task<User?> GetByIdAsync(UserId userId);
}
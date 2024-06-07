using Core.Application.Common.Users;
using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.Users.Repositories;

public class UsersQueryRepository(ApplicationDbContext dbContext) : IUsersQueryRepository
{
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await dbContext.Set<User>().AnyAsync(user => user.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await dbContext.Set<User>().FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> GetByIdAsync(UserId userId)
    {
        return await dbContext.Set<User>().FirstOrDefaultAsync(user => user.Id == userId);
    }
}
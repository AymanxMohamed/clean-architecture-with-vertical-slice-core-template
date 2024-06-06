using Core.Application.Common.Users;
using Core.Domain.Aggregates.UserAggregate;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.Users.Repositories;

public class UsersCommandRepository(ApplicationDbContext dbContext) : IUsersCommandRepository
{
    public async Task CreateAsync(User user)
    {
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();
    }
}
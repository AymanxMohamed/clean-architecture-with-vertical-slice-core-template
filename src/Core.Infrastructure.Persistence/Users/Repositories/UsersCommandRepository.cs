using Core.Application.Common.Users;
using Core.Domain.Aggregates.UserAggregate;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.Users.Repositories;

public class UsersCommandRepository(ApplicationDbContext dbContext) : IUsersCommandRepository
{
    public async Task CreateAsync(User user)
    {
        await dbContext.AddAsync(user);
    }

    public void UpdateAsync(User user)
    {
        dbContext.Update(user);
    }
}
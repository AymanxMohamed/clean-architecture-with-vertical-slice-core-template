using ProjectName.Domain.Common.Entities;

namespace ProjectName.Application.Common.Contexts;

public interface IUserContextService
{
    ErrorOr<UserContext> GetUserContext();
}

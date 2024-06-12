using Core.Domain.Common.Entities;

namespace Core.Application.Common.Contexts;

public interface IUserContextService
{
    ErrorOr<UserContext> GetUserContext();
}

using ProjectName.Application.Authentication.Dtos;
using ProjectName.Application.Common.Mediatr.Messages.Queries;

namespace ProjectName.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IQuery<AuthenticationResult>;

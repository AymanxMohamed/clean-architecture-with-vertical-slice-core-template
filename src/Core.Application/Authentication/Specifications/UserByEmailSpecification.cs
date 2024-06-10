using Core.Application.Common.Persistence;
using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Core.Application.Authentication.Specifications;

public class UserByEmailSpecification(string email) : SpecificationBase<User, UserId>(x => x.Email == email);
using ProjectName.Application.Common.Persistence;
using ProjectName.Domain.Aggregates.UserAggregate;
using ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;

namespace ProjectName.Application.Authentication.Specifications;

public class UserByEmailSpecification(string email) : SpecificationBase<User, UserId>(user => user.Email == email);
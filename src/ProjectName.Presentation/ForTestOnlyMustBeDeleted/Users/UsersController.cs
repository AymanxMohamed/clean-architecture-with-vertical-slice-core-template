using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ProjectName.Application.Authentication.Specifications;
using ProjectName.Application.Common.Persistence;
using ProjectName.Domain.Aggregates.UserAggregate;
using ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;
using ProjectName.Presentation.Common.Controllers;

namespace ProjectName.Presentation.ForTestOnlyMustBeDeleted.Users;

/// <summary>
/// This Controller is for testing purposes only.
/// </summary>
public class UsersController(ISender sender, IMapper mapper, 
    IGenericRepository<User, UserId> cachedGenericRepository,
    IGenericRepository<User, UserId> genericRepository) : 
    ApiController(sender, mapper)
{
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll([FromQuery] bool? withoutCaching, CancellationToken cancellationToken)
    {
        if (withoutCaching is not null && withoutCaching.Value)
        {
            return await genericRepository.ListAllAsync(cancellationToken);
        }
        
        return await cachedGenericRepository.ListAllAsync(cancellationToken);
    }
    
    [HttpGet("{email}")]
    public async Task<ActionResult<User>> Get(string email)
    {
        var user = await cachedGenericRepository.GetFirstOrDefault(new UserByEmailSpecification(email));

        return user is null ? NotFound() : Ok(user);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> Get(Guid id)
    {
        var user = await cachedGenericRepository.GetByIdAsync(UserId.Create(id));

        return user is null ? NotFound() : Ok(user);
    }
}
using Core.Application.Authentication.Specifications;
using Core.Application.Common.Persistence;
using Core.Application.Common.Services;
using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;
using Core.Presentation.Common.Controllers;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace Core.Presentation.Users;

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
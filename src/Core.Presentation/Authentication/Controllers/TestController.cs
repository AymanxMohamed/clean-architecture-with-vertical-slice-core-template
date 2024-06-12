using Core.Application.Common.Contexts;
using Core.Presentation.Common.Controllers;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Presentation.Authentication.Controllers;

[Authorize]
public class TestController(ISender sender, IMapper mapper, IUserContextService userContextService) : ApiController(sender, mapper)
{
    public IActionResult Test()
    {
        var result = userContextService.GetUserContext();

        return result.Match(Ok, Problem);
    }
}
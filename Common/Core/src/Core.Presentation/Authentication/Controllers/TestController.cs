using Core.Presentation.Common.Controllers;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Presentation.Authentication.Controllers;

[Authorize]
public class TestController(ISender sender, IMapper mapper) : ApiController(sender, mapper)
{
    [HttpGet]
    public IActionResult TestAuthorization()
    {
        return Ok();
    }
}
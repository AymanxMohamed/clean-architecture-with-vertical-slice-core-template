using Core.Presentation.Common.Controllers;

using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SampleProject.Application.Authentication.Commands.RegisterCommand;

namespace SampleProject.Presentation.FeatureName.Controllers;

[Authorize]
public class TestsController(ISender sender, IMapper mapper) : ApiController(sender, mapper)
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var command = new RegisterCommand(Username: "Ayman", Password: "12314125");
        var result = await _sender.Send(command);
        return result.Match(
            onValue: value => Ok(value),
            onError: Problem); 
    }

    [HttpPost]
    public IActionResult Create()
    {
        List<Error> errors = [
            Error.Validation("Validation", "Data Is missing"),
            Error.Validation("Validation", "Data Is missing"),
            Error.Validation("Validation", "Data Is missing"),
            Error.Validation("Validation", "Data Is missing"),
            Error.Validation("Validation", "Data Is missing"),
            Error.Validation("Validation", "Data Is missing"),
            Error.Validation("Validation", "Data Is missing")
        ];
        
        return Problem(errors);
    }
}
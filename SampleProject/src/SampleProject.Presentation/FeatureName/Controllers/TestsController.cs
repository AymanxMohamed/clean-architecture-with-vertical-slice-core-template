using Core.Presentation.Common.Controllers;

using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleProject.Presentation.FeatureName.Controllers;

[Authorize]
public class TestsController(ISender sender, IMapper mapper) : ApiController(sender, mapper)
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await Task.CompletedTask;
        
        return Ok();
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
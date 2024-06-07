using Core.Presentation.Common.Constants.HttpConstants;
using Core.Presentation.Common.Extensions;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Core.Presentation.Common.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class ApiController(ISender sender, IMapper mapper) : ControllerBase
{
    protected readonly ISender _sender = sender;
    protected readonly IMapper _mapper = mapper;

    [NonAction]
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        return errors.IsValidationErrors() 
            ? ValidationProblemFromErrors(errors) 
            : FirstErrorProblemFromErrors(errors);
    }
    
    [NonAction]
    private ActionResult ValidationProblemFromErrors(IEnumerable<Error> errors) => 
        ValidationProblem(errors.ToModelStateDictionary());
    
    [NonAction]
    private ObjectResult FirstErrorProblemFromErrors(List<Error> errors)
    {
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        return ProblemFromError(errors.FirstError());
    }
    
    [NonAction]
    private ObjectResult ProblemFromError(Error error) => Problem(
        statusCode: error.ToStatusCode(), 
        title: error.Description);
}
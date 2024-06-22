using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ProjectName.Infrastructure.Persistence.Common.Constants.Endpoints;

namespace ProjectName.Presentation.Common.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public class ApiErrorsController(ILogger<ApiErrorsController> logger) : ControllerBase
{
    [Route(template: CoreEndpoints.GlobalErrorHandlingEndPoint)]
    [HttpGet]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        
        logger.LogError(exception, "Exception occured: {Message}, {@Exception}", exception?.Message, exception);
        
        var (statusCode, message) = exception switch
        {
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occured.")
        };
        
        return Problem(statusCode: statusCode, detail: message);
    }
}
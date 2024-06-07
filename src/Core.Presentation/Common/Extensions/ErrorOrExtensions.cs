using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Presentation.Common.Extensions;

public static class ErrorOrExtensions
{
    public static ModelStateDictionary ToModelStateDictionary(this IEnumerable<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return modelStateDictionary;
    }

    public static Error FirstError(this IEnumerable<Error> errors) => errors.FirstOrDefault();
    
    public static int ToStatusCode(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    public static bool IsValidationErrors(this IEnumerable<Error> errors) =>
        errors.All(err => err.Type == ErrorType.Validation);
}
namespace Core.Domain.Common.Errors;

public static partial class Errors
{
    public static class UserContext
    {
        public static Error InvalidUserId => Error.Unexpected(
            code: "UserContext.InvalidUserId",
            description: "The Provided user id is not valid");
        
        public static Error NotFound => Error.NotFound(
            code: "UserContext.NotFound",
            description: "User claims can't be found");
    }
}
namespace Core.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "User is already exists");

        public static Error GenerateDuplicateEmailError(string email) => Error.Conflict(
            code: "User.DuplicateEmail",
            description: $"User with this email: {email} is already exists");
    }
}
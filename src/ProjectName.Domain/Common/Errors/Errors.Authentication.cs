namespace ProjectName.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Auth.InvalidCredentials",
            description: "Invalid Credentials");

        public static Error TooWeekPassword => Error.Validation(
            code: "Auth.TooWeekPassword",
            description: "Password must be at least 8 chars with 1 lower case, 1 upper case and 1 special char letters");
    }
}
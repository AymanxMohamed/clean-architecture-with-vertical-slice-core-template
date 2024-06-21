namespace ProjectName.Domain.Common.Errors;

public static partial class Errors
{
    public static class Email
    {
        public static readonly Error FileNotFound = Error.Validation(
            code: "Email.FileNotFound", 
            description: "The file specified in the path not found");
    
        public static readonly Error ServerNotSupported = Error.Validation(
            code: "Email.ServerNotSupported", 
            description: "The specified server not exists in the supported servers list");
    
        public static readonly Error MissingParticipant = Error.Validation(
            code: "Email.MissingParticipant", 
            description: "Email to participant must be provided");
    }
}
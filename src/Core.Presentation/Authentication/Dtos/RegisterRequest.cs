namespace Core.Presentation.Authentication.Dtos;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);
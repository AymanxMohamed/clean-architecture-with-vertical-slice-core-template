namespace ProjectName.Presentation.Authentication.Dtos;

public record LoginRequest(
    string Email,
    string Password);
using Core.Application.Mediatr.Messages.Commands;

namespace SampleProject.Application.Authentication.Commands.RegisterCommand;

public record RegisterCommand(string Username, string Password) : ICommand;



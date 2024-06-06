using Core.Application.Mediatr.Messages.Commands;

using ErrorOr;

namespace SampleProject.Application.Authentication.Commands.RegisterCommand;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    public async Task<ErrorOr<Success>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            return Error.Validation("Authentication.Username", "Username is empty");
        }

        return Result.Success;
    }
}
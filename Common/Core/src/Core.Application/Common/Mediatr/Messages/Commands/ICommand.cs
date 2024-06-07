using MediatR;

namespace Core.Application.Common.Mediatr.Messages.Commands;

public interface ICommand : IRequest<ErrorOr<Success>>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ErrorOr<Success>>
    where TCommand : ICommand;

public interface ICommand<TResult> : IRequest<ErrorOr<TResult>>;

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, ErrorOr<TResult>> 
    where TCommand : ICommand<TResult>;








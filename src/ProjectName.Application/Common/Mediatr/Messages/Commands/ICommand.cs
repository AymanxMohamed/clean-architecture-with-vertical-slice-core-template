using MediatR;

namespace ProjectName.Application.Common.Mediatr.Messages.Commands;

public interface ICommand : ICommandMarker, IRequest<ErrorOr<Success>>;

public interface ICommandMarker;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ErrorOr<Success>>
    where TCommand : ICommand;

public interface ICommand<TResult> : ICommandMarker, IRequest<ErrorOr<TResult>>;

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, ErrorOr<TResult>> 
    where TCommand : ICommand<TResult>;








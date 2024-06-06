using ErrorOr;

using MediatR;

namespace Core.Application.Mediatr.Messages.Queries;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ErrorOr<TResponse>>
    where TQuery : IQuery<TResponse>;

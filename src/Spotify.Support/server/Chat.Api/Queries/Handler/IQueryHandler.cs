using Chat.Domain;
using MongoDB.Bson;

namespace Chat.Api.Queries.Handler;

public interface IQueryHandler<in TQuery, out TResult>
    where TQuery : IQuery<TResult>
{
    Task<Meta> Handle(TQuery query);
}
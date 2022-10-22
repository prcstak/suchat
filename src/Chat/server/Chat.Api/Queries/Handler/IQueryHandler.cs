using MongoDB.Bson;

namespace Chat.Api.Queries.Handler;

public interface IQueryHandler<in TQuery, out TResult>
    where TQuery : IQuery<TResult>
{
    Task<BsonDocument> Handle(TQuery query);
}
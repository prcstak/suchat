using MongoDB.Bson;

namespace Chat.Api.Queries;

public record GetMetaQuery(string Filename) : IQuery<BsonDocument>;
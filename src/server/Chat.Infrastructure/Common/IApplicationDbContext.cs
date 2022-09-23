using MongoDB.Driver;
using Shop.Domain;

namespace Chat.Infrastructure.Common;

public interface IApplicationDbContext
{
    IMongoCollection<User> User { get; }
    IMongoCollection<Message> Message { get; }
}

using Chat.Domain;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Common;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Message> Messages { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
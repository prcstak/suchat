using Chat.Domain;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Message> Messages { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
using Chat.Application.Common.Dto;
using Chat.Application.Common.Interfaces;
using Chat.Domain;
using Chat.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application;

public class MessageService : IMessageService
{
    private readonly IApplicationDbContext _context;

    public MessageService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Message> AddAsync(AddMessageDto addMessageDto, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            Body = addMessageDto.Body,
            Created = DateTime.Now,
            UserId = addMessageDto.UserId
        };

        await _context.Messages.AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return message;
    }

    public async Task<GetMessagesList> GetMessageHistory(int offset, int limit, Guid groupId, string userId,
        CancellationToken cancellationToken)
    {
        var messages = await _context.Messages
            .AsNoTracking()
            .Include(i => i.User)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return GetMessagesList.MapFrom(messages);
    }
}
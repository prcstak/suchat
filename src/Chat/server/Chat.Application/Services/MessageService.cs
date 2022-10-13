using Chat.Application.Interfaces;
using Chat.Common.Dto;
using Chat.Domain;
using Chat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Services;

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
            Username = addMessageDto.Username
        };

        await _context.Messages.AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return message;
    }

    public async Task<GetMessagesList> GetMessageHistory(int offset, int limit,
        CancellationToken cancellationToken)
    {
        var messages = await _context.Messages
            .AsNoTracking()
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return GetMessagesList.MapFrom(messages);
    }
}
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
            Created = DateTime.Now.ToString("s"),
            Username = addMessageDto.Username
        };
        Console.WriteLine(message.Created);

        await _context.Messages.AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return message;
    }

    public async Task<GetMessagesList> GetMessageHistory(int offset, int limit,
        CancellationToken cancellationToken)
    {
        var messages = await _context.Messages
            .AsNoTracking()
            .OrderByDescending(t => t.Created)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
        Console.WriteLine(messages.FirstOrDefault());
        return GetMessagesList.MapFrom(messages);
    }
}
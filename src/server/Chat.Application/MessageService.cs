using Chat.Application.Common.Dto;
using Chat.Application.Common.Interfaces;
using Chat.Infrastructure.Common;
using Shop.Domain;

namespace Chat.Application;

public class MessageService : IMessageService
{
    private readonly IApplicationDbContext _context;

    public MessageService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AddMessageDto postMessageDto, CancellationToken cancellationToken)
    {
        await _context.Message.InsertOneAsync(new Message
        {
            Body = postMessageDto.Body,
            Created = default,
        }, cancellationToken: cancellationToken);
    }

    public Task GetMessageHistory(int offset, int limit, int groupId, int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException(); // TODO
    }
}
using Chat.Application.Common.Dto;
using Chat.Application.Common.Interfaces;
using Chat.Infrastructure.Common;

namespace Chat.Application;

public class MessageService : IMessageService
{
    private readonly IApplicationDbContext _context;

    public MessageService(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(AddMessageDto postMessageDto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task GetMessageHistory(int offset, int limit, int groupId, int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
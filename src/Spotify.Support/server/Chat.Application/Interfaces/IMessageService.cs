using Chat.Common.Dto;
using Chat.Domain;

namespace Chat.Application.Interfaces;

public interface IMessageService
{
    Task<Message> AddAsync(AddMessageDto postMessageDto, CancellationToken cancellationToken);
    Task<GetMessagesList> GetMessageHistory(
        int offset, int limit, string room,
        CancellationToken cancellationToken);
}
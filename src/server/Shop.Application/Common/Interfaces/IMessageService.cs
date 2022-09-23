using Shop.Application.Common.Dto;

namespace Shop.Application.Common.Interfaces;

public interface IMessageService
{
    Task AddAsync(AddMessageDto postMessageDto, CancellationToken cancellationToken);
    Task GetMessageHistory(int offset, int limit, int groupId, int userId, CancellationToken cancellationToken);
}
using Chat.Application.Common.Dto;
using Shop.Domain;

namespace Chat.Application.Common.Interfaces;

public interface IUserService
{
    Task<User> AddAsync(AddUserDto addUserDto, CancellationToken cancellationToken);
    Task<GetUserDtoVm> GetUserAsync(GetUserDto getUserDto, CancellationToken cancellationToken);
}
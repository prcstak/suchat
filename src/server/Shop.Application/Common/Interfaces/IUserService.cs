using Shop.Application.Common.Dto;
using Shop.Domain;

namespace Shop.Application.Common.Interfaces;

public interface IUserService
{
    Task<User> AddAsync(AddUserDto addUserDto, CancellationToken cancellationToken);
    Task<GetUserDtoVm> GetUserAsync(GetUserDto getUserDto, CancellationToken cancellationToken);
}
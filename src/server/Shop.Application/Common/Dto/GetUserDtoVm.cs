using Shop.Domain;

namespace Shop.Application.Common.Dto;

public record GetUserDtoVm(string Username, string Id)
{
    public static GetUserDtoVm MapFrom(User user)
        => new GetUserDtoVm(user.Username, user.Id);
};
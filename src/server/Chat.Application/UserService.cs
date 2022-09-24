using Chat.Application.Common.Dto;
using Chat.Application.Common.Exceptions;
using Chat.Application.Common.Interfaces;
using Chat.Infrastructure.Common;
using MongoDB.Driver;
using Shop.Domain;

namespace Chat.Application;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(AddUserDto addUserDto, CancellationToken cancellationToken)
    {
        var user = await _context.User
            .Find(user => user.Username == addUserDto.Username).SingleOrDefaultAsync(cancellationToken: cancellationToken);;

        if (user != null)
            throw new AlreadyExist(nameof(User), addUserDto.Username); // result based handling?

        var newUser = new User
        {
            Username = addUserDto.Username,
            Password = addUserDto.Password // TODO: hash + salt or DGAF?
        };
        
        await _context.User.InsertOneAsync(newUser, cancellationToken: cancellationToken);

        return newUser;
    }

    public async Task<GetUserDtoVm> GetUserAsync(GetUserDto getUserDto, CancellationToken cancellationToken)
    {
        var user = await _context.User
            .Find(user => user.Username == getUserDto.Username 
                               && user.Password == getUserDto.Password).FirstAsync(cancellationToken);
        if (user == null)
            throw new NotFoundException(nameof(User));
        
        return GetUserDtoVm.MapFrom(user);
    }
}
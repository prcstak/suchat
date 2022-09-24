using Chat.Domain;

namespace Chat.Api.Models;

public record RegisterUserRequest(string Username, string Password, UserRoles Role);
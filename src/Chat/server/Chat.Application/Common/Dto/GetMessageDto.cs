using Chat.Domain;

namespace Chat.Application.Common.Dto;

public record GetMessageDto(
    Guid Id,
    string Body,
    DateTime Created,
    string Username)
{
    public static GetMessageDto MapFrom(Message message)
        => new(message.Id, message.Body, message.Created, message.Username);
};
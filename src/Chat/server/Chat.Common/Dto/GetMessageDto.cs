using Chat.Domain;

namespace Chat.Common.Dto;

public record GetMessageDto(
    Guid Id,
    string Body,
    string Created,
    string Username,
    bool IsFile)
{
    public static GetMessageDto MapFrom(Message message)
        => new(message.Id, message.Body, message.Created, message.Username, message.IsFile);
};
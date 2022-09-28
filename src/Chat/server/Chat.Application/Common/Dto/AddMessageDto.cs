namespace Chat.Application.Common.Dto;

public record AddMessageDto(string UserId, Guid GroupId, string Body);
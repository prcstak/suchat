namespace Chat.Common.Dto;

public record AddMessageDto(string Username, string Body, bool IsFile);
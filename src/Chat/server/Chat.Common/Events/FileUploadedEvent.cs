namespace Chat.Common.Events;

public record FileUploadedEvent(string Filename, Guid RequestId); 
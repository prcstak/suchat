namespace Chat.Common.Events;

public record MediaUploadedEvent(string Filename, string RequestId) : IEvent;
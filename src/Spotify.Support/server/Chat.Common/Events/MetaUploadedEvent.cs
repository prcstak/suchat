namespace Chat.Common.Events;

public record MetaUploadedEvent(Guid RequestId, string Filename, string Room) : IEvent;
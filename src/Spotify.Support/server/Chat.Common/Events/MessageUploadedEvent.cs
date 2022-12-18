namespace Chat.Common.Events;

public record MessageUploadedEvent(string Username, string Body, bool IsFile, string Room) :  IEvent { }
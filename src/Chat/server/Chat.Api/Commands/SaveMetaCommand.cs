namespace Chat.Api.Commands;

public record SaveMetaCommand(string MetaJson, string Filename, Guid RequestId);
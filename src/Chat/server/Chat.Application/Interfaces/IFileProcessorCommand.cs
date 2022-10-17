namespace Chat.Application.Interfaces;

public interface IFileProcessCommand
{
    IReadOnlyList<MetadataExtractor.Directory> GetMeta();
}
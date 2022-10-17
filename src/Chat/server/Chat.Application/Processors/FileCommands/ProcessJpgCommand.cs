using Chat.Application.Interfaces;
using MetadataExtractor.Formats.Jpeg;
using Directory = MetadataExtractor.Directory;

namespace Chat.Application.Processors.FileCommands;

public class ProcessJpgCommand : IFileProcessCommand
{
    private string _path;
    public ProcessJpgCommand(string path)
    {
        _path = path;
    }
    
    public IReadOnlyList<Directory> GetMeta()
    {
        var meta = JpegMetadataReader.ReadMetadata(_path);
        // ... extract needed meta (there's a lot, actually)
        return meta;
    }
}
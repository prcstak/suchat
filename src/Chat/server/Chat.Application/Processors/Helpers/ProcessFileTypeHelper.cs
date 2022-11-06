using MetadataExtractor.Formats.Jpeg;
using Directory = MetadataExtractor.Directory;

namespace Chat.Application.Processors.Helpers;

public static class ProcessFileTypeHelper
{
    public static IReadOnlyList<Directory> GetJpegMetadata(string filepath)
    {
        var meta = JpegMetadataReader.ReadMetadata(filepath);
        // ... extract needed meta (there's a lot, actually)
        return meta;
    }
}
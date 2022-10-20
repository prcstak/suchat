using Microsoft.AspNetCore.Http;
using Directory = MetadataExtractor.Directory;

namespace Chat.Application.Interfaces;

public interface IFileProcessor
{
    Task<IReadOnlyList<Directory>?> ExtractMetadataAsync(Stream stream, IFormFile file);
}
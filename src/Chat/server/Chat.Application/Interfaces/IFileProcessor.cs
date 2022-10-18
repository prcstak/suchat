using Microsoft.AspNetCore.Http;

namespace Chat.Application.Interfaces;

public interface IFileProcessor
{
    IReadOnlyList<MetadataExtractor.Directory>? ExtractMetadataAsync(Stream stream, IFormFile file);
}
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Interfaces;

public interface IFileProcessor
{
    Task<IReadOnlyList<MetadataExtractor.Directory>?> ExtractMetadataAsync(IFormFile file);
}
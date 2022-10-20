using Chat.Application.Interfaces;
using Chat.Application.Processors.Helpers;
using Chat.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Directory = MetadataExtractor.Directory;

namespace Chat.Application.Processors;

public class FileProcessor : IFileProcessor
{
    public IReadOnlyList<Directory>? ExtractMetadataAsync(
        Stream stream, 
        IFormFile file)
    {
        var metaData = GetMeta(stream, file);

        return metaData;
    }

    private static string? GetExtension(string contentType)
        => contentType.Split('/').Last();

    private static IReadOnlyList<MetadataExtractor.Directory>? GetMeta(Stream stream, IFormFile file) 
        => GetExtension(file.ContentType) switch 
        {
            "jpeg" => ProcessFileTypeHelper.GetJpegMetadata(stream),
            _ => throw new FileExtensionException()
        };
}
using Chat.Application.Interfaces;
using Chat.Application.Processors.Helpers;
using Chat.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Processors;

public class FileProcessor : IFileProcessor
{
    private static string? _cachePath;
    public FileProcessor()
    {
        _cachePath ??= Path.Combine(Directory.GetCurrentDirectory(), "_cache");
        if (!Directory.Exists(_cachePath))
            Directory.CreateDirectory(_cachePath);
    }
    public async Task<IReadOnlyList<MetadataExtractor.Directory>?> ExtractMetadataAsync(Stream stream,
        IFormFile file)
    {
        var filePath = Path.Combine(_cachePath!, file.FileName);
        await using (var fileStream = new FileStream(
                         filePath,
                         FileMode.Create,
                         FileAccess.Write))
        {
            await file.CopyToAsync(fileStream);  // must be explicitly in curly brackets
        }

        var metaData = GetMeta(file, filePath);

        RemoveTempFile(filePath);

        return metaData;
    }


    private static string? GetExtension(string contentType)
        => contentType.Split('/').Last();

    private static IReadOnlyList<MetadataExtractor.Directory> GetMeta(IFormFile file, string filepath) 
        => GetExtension(file.ContentType) switch 
        {
            "jpeg" => ProcessFileTypeHelper.GetJpegMetadata(filepath),
            _ => throw new FileExtensionException()
        };

    private static void RemoveTempFile(string filename)
        => File.Delete(Path.Combine(_cachePath!, filename));
}
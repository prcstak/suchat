using Chat.Application.Interfaces;
using Chat.Application.Processors.Helpers;
using Chat.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Processors;

public class FileProcessor : IFileProcessor
{
    private static readonly string[] SupportedExtensions = { "jpeg" };
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
            await file.CopyToAsync(fileStream); // must be explicitly in curly brackets
        }

        var metaData = GetMeta(file, filePath);

        RemoveTempFile(filePath);

        return metaData;
    }
    
    public bool IsSupportedExtension(string contentType)
        => SupportedExtensions.Contains(contentType.Split('/')[^1]);

    private static IReadOnlyList<MetadataExtractor.Directory> GetMeta(IFormFile file, string filepath)
    {
        var extension = file.ContentType.Split('/')[^1];
        return extension switch
        {
            "jpeg" => ProcessFileTypeHelper.GetJpegMetadata(filepath),
            _ => throw new FileExtensionException(extension)
        };
    }

    private static void RemoveTempFile(string filename)
        => File.Delete(Path.Combine(_cachePath!, filename));
}
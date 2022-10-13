using Chat.Application.Interfaces;
using Chat.Infrastructure.Interfaces;

namespace Chat.Application.Services;

public class FileService : IFileService
{
    private readonly IFileMetaDbContext _fileMetaDbContext;

    public FileService(IFileMetaDbContext fileMetaDbContext)
    {
        _fileMetaDbContext = fileMetaDbContext;
    }
}
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

public class FileController : BaseController
{
    private readonly IFileService _fileService;

    public FileController(
        IFileService fileService)
    {
        _fileService = fileService;
    }
}
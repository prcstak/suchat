using Chat.Api.Commands;
using Chat.Api.Commands.Handler;
using Chat.Api.Queries;
using Chat.Api.Queries.Handler;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

public class MetaController : BaseController
{
    private readonly MetaCommandHandler _metaCommandHandler;
    private readonly MetaQueryHandler _metaQueryHandler;

    public MetaController(
        MetaCommandHandler metaCommandHandler,
        MetaQueryHandler metaQueryHandler)
    {
        _metaCommandHandler = metaCommandHandler;
        _metaQueryHandler = metaQueryHandler;
    }

    [HttpPost]
    public async Task<IActionResult> SaveMeta(
        string metaJson, 
        string filename, 
        Guid requestId,
        string author,
        string room)
    {
        var command = new SaveMetaCommand(metaJson, filename, requestId, author, room);
        await _metaCommandHandler.Handle(command);

        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMeta(string filename)
    {
        var query = new GetMetaQuery(filename);
        var meta = await _metaQueryHandler.Handle(query);

        return Ok(meta);
    }
}
using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Common.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

public class MessageController : BaseController
{
    private readonly IMessageService _messageService;
    private readonly IMessageProducer _messageProducer;

    public MessageController(
        IMessageService messageService, 
        IMessageProducer messageProducer)
    {
        _messageService = messageService;
        _messageProducer = messageProducer;
    }
    
    [HttpPost]
    [Route("history")]
    public async Task<IActionResult> GetHistory(
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetMessageHistory(
            offset,
            limit, 
            cancellationToken);
        
        return Ok(messages);
    }
}
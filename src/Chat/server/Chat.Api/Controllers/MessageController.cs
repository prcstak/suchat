using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Common.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

public class MessageController : BaseController
{
    private readonly IMessageService _messageService;
    private readonly IBrokerProducer _messageProducer;

    public MessageController(
        IMessageService messageService, 
        IBrokerProducer messageProducer)
    {
        _messageService = messageService;
        _messageProducer = messageProducer;
    }

    [HttpPost]
    public async Task<IActionResult> PostMessage(
        AddMessageDto addMessageDto,
        CancellationToken cancellationToken)
    {
        _messageProducer.SendMessage(addMessageDto);
        
        return Ok();
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
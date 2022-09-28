using Chat.Api.Producer;
using Microsoft.AspNetCore.Mvc;
using Chat.Application.Common.Dto;
using Chat.Application.Common.Interfaces;

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
    public async Task<IActionResult> PostMessage(
        AddMessageDto addMessageDto,
        CancellationToken cancellationToken)
    {
        await _messageService.AddAsync(addMessageDto, cancellationToken);
        
        return Ok();
    }

    [HttpPost]
    [Route("TEST")]
    public async Task<IActionResult> TestMq(
        AddMessageDto addMessageDto,
        CancellationToken cancellationToken)
    {
        _messageProducer.SendMessage(addMessageDto);
        
        return Ok();
    }
}
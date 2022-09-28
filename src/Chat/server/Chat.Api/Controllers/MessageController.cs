using Chat.Api.Producer;
using Microsoft.AspNetCore.Mvc;
using Chat.Application.Common.Dto;
using Chat.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Chat.Api.Controllers;

[Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
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
        _messageProducer.SendMessage(addMessageDto);
        
        return Ok();
    }

    [HttpPost]
    [Route("history")]
    public async Task<IActionResult> GetHistory(
        int offset,
        int limit,
        Guid chatId,
        string userId,
        CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetMessageHistory(
            offset,
            limit, 
            chatId, 
            userId, 
            cancellationToken);
        
        return Ok(messages);
    }
}
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Common.Dto;
using Shop.Application.Common.Interfaces;

namespace Chat.Api.Controllers;

public class MessageController : BaseController
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost]
    public async Task<IActionResult> PostMessage(AddMessageDto addMessageDto, CancellationToken cancellationToken)
    {
        await _messageService.AddAsync(addMessageDto, cancellationToken);
        
        return Ok();
    }
}
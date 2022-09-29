﻿using Chat.Domain;

namespace Chat.Application.Common.Dto;

public record GetMessagesList(IList<GetMessageDto> Messages)
{
    public static GetMessagesList MapFrom(IEnumerable<Message> messages)
        => new (messages
            .Select(GetMessageDto.MapFrom)
            .ToList());
};

namespace Chat.Api.Commands.Handler;

public interface ICommandHandler<in TCommand>
{
    Task Handle(TCommand command);
}
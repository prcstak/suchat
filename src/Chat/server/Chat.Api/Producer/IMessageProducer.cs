namespace Chat.Api.Producer;

public interface IBrokerProducer
{
    void SendMessage<T> (T message);
}
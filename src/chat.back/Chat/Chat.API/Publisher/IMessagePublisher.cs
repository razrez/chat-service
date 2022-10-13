namespace Chat.API.Publisher;

public interface IMessagePublisher
{
    void SaveMessage<T>(T message);
}


namespace Chat.API.Publisher;

public interface IMessagePublisher
{
    void SaveMessage<T>(T message);
    void UploadFileOrMeta<T>(T data, string queueName);
}


namespace Chat.AppCore.Publisher;

public interface IMessagePublisher
{
    void SaveMessage<T>(T message);
    void UploadFileOrMeta<T>(T data, string queueName);
    void UpdateStatistic<T>(T data, string queueName);
}
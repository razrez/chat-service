namespace Chat.DB.Data.Repository;

public interface IChatRepository
{
    Task GetHistory(string room);
    Task SaveMessage(string room, string user, string message);
}
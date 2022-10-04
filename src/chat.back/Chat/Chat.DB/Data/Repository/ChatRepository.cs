namespace Chat.DB.Data.Repository;

public class ChatRepository : IChatRepository
{
    public Task GetHistory(string room)
    {
        throw new NotImplementedException();
    }

    public Task SaveMessage(string room, string user, string message)
    {
        throw new NotImplementedException();
    }
}
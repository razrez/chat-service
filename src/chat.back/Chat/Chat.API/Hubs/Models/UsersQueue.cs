using System.Collections.Concurrent;

namespace Chat.API.Hubs.Models;

public class UsersQueue
{
    private static Queue<UserConnection> _concurrentQueue = new();

    public override string? ToString()
    {
        return _concurrentQueue.ToString();
    }

    public void AddUser(UserConnection userConnection)
    {
        _concurrentQueue.Enqueue(userConnection);
    }

    /// <summary>
    /// Returns Room's name to connect to the user or empty string
    /// </summary>
    /// <returns></returns>
    public string HelpUser()
    {
        _concurrentQueue.TryDequeue(out UserConnection? result);
        return result != null ? result.Room : "";
    }
}
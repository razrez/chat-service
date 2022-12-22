using System.Collections.Concurrent;

namespace Chat.API.Hubs.Models;

public class UsersQueue
{
    private static readonly ConcurrentQueue<string> Queue = new();

    public override string? ToString()
    {
        return Queue.ToArray().ToString();
    }

    public void AddUser(string userName)
    {
        if (Queue.Contains(userName)) return;
        Queue.Enqueue(userName);
    }

    /// <summary>
    /// Returns Room's name to connect to the user or empty string
    /// </summary>
    /// <returns></returns>
    public string HelpUser()
    {
        Queue.TryDequeue(out string? result);
        return result ?? "";
    }
}
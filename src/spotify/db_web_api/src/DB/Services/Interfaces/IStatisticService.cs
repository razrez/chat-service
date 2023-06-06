namespace DB.Services.Interfaces;

public interface IStatisticService
{
    Task<List<object>> GetAll();
    Task<bool> Add(int songId);
}
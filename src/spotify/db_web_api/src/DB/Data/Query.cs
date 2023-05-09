using DB.Models;

namespace DB.Data;

public class Query
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Song?> GetSongs([Service] SpotifyContext context)
    {
        return context.Songs.AsQueryable();
    }

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Genre?> GetGenres([Service] SpotifyContext context) => context.Genres.AsQueryable();
}
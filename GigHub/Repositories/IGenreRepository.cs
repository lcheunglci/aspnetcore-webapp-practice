using GigHub.Models;

namespace GigHub.Repositories
{
    public interface IGenreRepository
    {
        Genre GetGenre(byte genre);
        IEnumerable<Genre> GetGenres();
    }
}
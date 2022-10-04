using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IGenreRepository
    {
        Genre GetGenre(byte genre);
        IEnumerable<Genre> GetGenres();
    }
}
﻿using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;

namespace GigHub.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public Genre GetGenre(byte genre)
        {
            return _context.Genres.Single(g => g.Id == genre);
        }

    }
}

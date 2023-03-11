using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public class MovieService : IMoviesService
    {
        private readonly ApplicationDbContext context;

        public MovieService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            return await context.Movies.Include(m => m.Genere)
                               .Where(m => m.GenereId == genreId || genreId == 0)
                               .OrderByDescending(m => m.Rate)
                               .ToListAsync();
            
        }

        public async Task<Movie> GetById(int id)
        {
           return await context.Movies.Include(m => m.Genere).SingleOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Movie> Add(Movie movie)
        {
            await context.Movies.AddAsync(movie);
            context.SaveChanges();
            return movie;
        }

        public Movie Update(Movie movie)
        {
            context.Movies.Update(movie);
            context.SaveChanges();
            return movie;
        }
        public Movie Delete(Movie movie)
        {
            context.Movies.Remove(movie);
            context.SaveChanges();
            return movie;
        }

    }
}

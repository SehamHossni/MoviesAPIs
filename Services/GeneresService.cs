using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public class GeneresService : IGeneresService
    {
        private readonly ApplicationDbContext context;

        public GeneresService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Genere>> GetAll()
        {
           return await context.Generes.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genere> GetById(byte id)
        {
            return await context.Generes.SingleOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Genere> Add(Genere Genere)
        {
            await context.Generes.AddAsync(Genere);
            context.SaveChanges();
            return Genere;
        }

        public Genere Update(Genere Genere)
        {
            context.Update(Genere);
            context.SaveChanges();
            return Genere;
        }

        public Genere Delete(Genere Genere)
        {
            context.Remove(Genere);
            context.SaveChanges();
            return Genere;
        }

        public Task<bool> IsValidGenre(byte id)
        {
            return  context.Generes.AnyAsync(g => g.Id == id);
        }
    }
}

using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public interface IGeneresService
    {
        Task<IEnumerable<Genere>> GetAll();
        Task<Genere> GetById(byte id);
        Task<Genere> Add(Genere Genere);
        Genere Update(Genere Genere);
        Genere Delete(Genere Genere);
        Task<bool> IsValidGenre(byte id);
    }
}

using Microsoft.EntityFrameworkCore;
using _6._Films._Razor_Pages.Model;

namespace _6._Films._Razor_Pages.Repository
{
    public interface IRepository
    {
        Task<IList<Film>> GetFilms();
        Task<Film> GetFilm(int id);
        bool IsNameExists(string name);
        Task Add(Film item);
        void Update(Film item);
        Task Delete(int id);
        Task Save();
    }
    public class Repository : IRepository
    {
        private readonly FilmContext db;
        public Repository(FilmContext context) => db = context;
        public async Task<IList<Film>> GetFilms() => await db.Films.ToListAsync();
        public async Task<Film> GetFilm(int id) => await db.Films.FindAsync(id);
        public bool IsNameExists(string name) => db.Films.Any(f => f.Name == name);
        public async Task Add(Film film) => await db.Films.AddAsync(film);
        public void Update(Film film) => db.Entry(film).State = EntityState.Modified;
        public async Task Delete(int id) => db.Films.Remove(await GetFilm(id));
        public async Task Save() => await db.SaveChangesAsync();
    }
}

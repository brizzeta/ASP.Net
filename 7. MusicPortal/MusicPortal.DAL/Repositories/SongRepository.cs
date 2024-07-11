using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.Context;
using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories {
    public class SongRepository : IRepository<Song> {
        private MainContext db;
        public SongRepository(MainContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<Song>> GetAll()
        {
            return await db.Songs.ToListAsync();
        }
        public async Task<Song> GetById(int songId)
        {
            return await db.Songs.FirstOrDefaultAsync(s => s.Id == songId);
        }
        public async Task<Song> GetByStr(string title)
        {
            return await db.Songs.FirstOrDefaultAsync(s => s.Title == title);
        }
        public async Task<bool> IsStr(string title)
        {
            return await db.Songs.AnyAsync(s => s.Title == title);
        }
        public async Task Add(Song song)
        {
            await db.Songs.AddAsync(song);
        }
        public void Update(Song song) 
        {
            var existingSong = db.Songs.Local.FirstOrDefault(s => s.Id == song.Id);
            if (existingSong != null)
            {
                // Если сущность уже отслеживается, обновляются свойства
                db.Entry(existingSong).CurrentValues.SetValues(song);
            }
            else
            {
                // Если сущность не отслеживается, присоединяется и установливается состояние как Modified
                db.Songs.Attach(song);
                db.Entry(song).State = EntityState.Modified;
            }
        }
        public async Task Delete(int songId)
        {
            var song = await db.Songs.FindAsync(songId);
            if (song == null)
            {
                song = new Song { Id = songId };
                db.Songs.Attach(song);
            }
            db.Songs.Remove(song);
            await db.SaveChangesAsync();
        }
    }
}
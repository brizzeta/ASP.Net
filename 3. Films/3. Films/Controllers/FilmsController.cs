using _3._Films.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _3._Films.Controllers
{
    public class FilmsController : Controller
    {
        private readonly FilmContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FilmsController(FilmContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            var films = await _context.Films.Include(f => f.Genres).ToListAsync();
            return View(films);
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.Include(f => f.Genres).FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Genres = _context.Genres
                                    .Select(g => new SelectListItem { Value = g.ID.ToString(), Text = g.Name })
                                    .ToList();
            return View();
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Director,Year,Description,Genres,Poster")] Film film, string genres, IFormFile uploadedFile)
        {
            if (ModelState.IsValid || (ModelState.ContainsKey("uploadedFile") && ModelState["uploadedFile"].ValidationState == ModelValidationState.Invalid))
            {
                film.Genres = new List<Genre>();
                if (!string.IsNullOrEmpty(genres))
                {
                    var genreNames = genres.Split(',').Select(g => g.Trim()).ToList();

                    foreach (var genreName in genreNames)
                    {
                        var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

                        if (existingGenre != null)
                        {
                            film.Genres.Add(existingGenre);
                        }
                        else
                        {
                            var newGenre = new Genre { Name = genreName };
                            film.Genres.Add(newGenre);
                        }
                    }
                }
                if (uploadedFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    film.Poster = "/img/" + uniqueFileName;
                }
                else film.Poster = "/img/default.jpg";

                _context.Attach(film);
                _context.Films.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }


        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.Include(f => f.Genres).FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            // Получаем список всех доступных жанров
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");

            return View(film);
        }


        // POST: Films/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Director,Year,Description,Genres,Poster")] Film film, string genres, IFormFile? uploadedFile)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid || (ModelState.ContainsKey("uploadedFile") && ModelState["uploadedFile"].ValidationState == ModelValidationState.Invalid))
            {
                try
                {
                    var existingFilm = await _context.Films
                        .Include(f => f.Genres)
                        .FirstOrDefaultAsync(f => f.Id == id);

                    if (existingFilm == null)
                    {
                        return NotFound();
                    }

                    existingFilm.Name = film.Name;
                    existingFilm.Director = film.Director;
                    existingFilm.Year = film.Year;
                    existingFilm.Description = film.Description;

                    existingFilm.Genres.Clear();

                    if (!string.IsNullOrEmpty(genres))
                    {
                        var genreNames = genres.Split(',').Select(g => g.Trim()).ToList();

                        foreach (var genreName in genreNames)
                        {
                            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

                            if (existingGenre != null)
                            {
                                existingFilm.Genres.Add(existingGenre);
                            }
                            else
                            {
                                var newGenre = new Genre { Name = genreName };
                                existingFilm.Genres.Add(newGenre);
                            }
                        }
                    }

                    if (uploadedFile != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        if (System.IO.File.Exists(filePath)) ModelState.AddModelError("Poster", "A file with the same name already exists");

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }
                        existingFilm.Poster = "/img/" + uniqueFileName;
                    }

                    _context.Update(existingFilm);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(film);
        }





        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.Include(g => g.Genres).FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
    }
}

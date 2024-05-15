using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _6._Films._Razor_Pages.Model;
using _6._Films._Razor_Pages.Repository;

namespace _6._Films._Razor_Pages.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository rep;
        private readonly IWebHostEnvironment baseDirectory;
        public Film? Film { get; set; } = default!;
        public DeleteModel(IRepository r, IWebHostEnvironment web)
        {
            rep = r;
            baseDirectory = web;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            Film = await rep.GetFilm((int)id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            var film = await rep.GetFilm((int)id);
            System.IO.File.Delete(Path.Combine(baseDirectory.WebRootPath, film.Poster!.TrimStart('/')));
            await rep.Delete((int)id);
            await rep.Save();
            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _6._Films._Razor_Pages.Model;
using _6._Films._Razor_Pages.Repository;

namespace _6._Films._Razor_Pages.Pages
{
    public class EditModel : PageModel
    {
        private readonly IRepository rep;
        private readonly IWebHostEnvironment baseDirectory;
        [BindProperty]
        public Film? Film { get; set; } = default!;
        public EditModel(IRepository r, IWebHostEnvironment web)
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
        public async Task<IActionResult> OnPostAsync(IFormFile uploadedFile)
        {
            if (!ModelState.IsValid && uploadedFile == null) return Page();
            string uploadsFolder = Path.Combine(baseDirectory.WebRootPath, "img");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            if (System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError("Poster", "Файл с таким именем уже существует.");
                return Page();
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create)) await uploadedFile.CopyToAsync(fileStream);
            Film!.Poster = "/img/" + uniqueFileName;
            rep.Update(Film!);
            await rep.Save();
            return RedirectToPage("./Index");
        }
    }
}

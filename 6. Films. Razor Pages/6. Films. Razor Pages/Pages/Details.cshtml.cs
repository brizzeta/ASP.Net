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
    public class DetailsModel : PageModel
    {
        private readonly IRepository rep;
        public Film? Film { get; set; } = default!;
        public DetailsModel(IRepository r) => rep = r;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            Film = await rep.GetFilm((int)id);
            return Page();
        }
    }
}

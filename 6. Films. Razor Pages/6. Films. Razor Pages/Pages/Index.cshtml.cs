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
    public class IndexModel : PageModel
    {
        public IList<Film> Film { get; set; } = default!;
        public async Task OnGetAsync([FromServices] IRepository rep) => Film = await rep.GetFilms();
    }
}

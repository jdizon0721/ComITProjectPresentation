using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoppingStore.Infrastructure;
using MyShoppingStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyShoppingStore.Controllers
{
    public class PagesController : Controller
    {
        private readonly MyShoppingStoreContext context;

        public PagesController(MyShoppingStoreContext context)
        {
            this.context = context;
        }

        //Get Page Slug
        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null)
            {
                return View(await context.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }
            Page page = await context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}

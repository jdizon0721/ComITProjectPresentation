using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoppingStore.Infrastructure;
using MyShoppingStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShoppingStore.Areas.admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly MyShoppingStoreContext context;

        public PagesController(MyShoppingStoreContext context)
        {
            this.context = context;
        }

        //Take /admin /Pages
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in context.Pages orderby p.Sorting select p;

            List<Page> pagesList = await pages.ToListAsync();

            return View(pagesList);
        }

        //Take /admin /Pages /details 
        public async Task<IActionResult> Details(int id)
        {
            Page page = await context.Pages.FirstOrDefaultAsync(x => x.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        //Take /admin /Pages /create 
        public IActionResult Create() => View();

        //POST /admin /Pages /create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists");
                    return View(page);
                }

                context.Add(page);
                await context.SaveChangesAsync();

                TempData["TitleContentCreated"] = "Page has been added";

                return RedirectToAction("Index");
            }
            return View(page);
        }

        //Take /admin /Pages /Edit 
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
        //POST /admin /Pages / Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id == 1 ? "home" : page.Slug = page.Title.ToLower().Replace(" ", "-");

                var slug = await context.Pages.Where(x => x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists");
                    return View(page);
                }

                context.Update(page);
                await context.SaveChangesAsync();

                TempData["TitleContentCreated"] = "Page has been edited";

                return RedirectToAction("Edit", new { id = page.Id });

            }

            return View(page);
        }
        //Take /admin /Pages / delete 
        public async Task<IActionResult> Delete(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "Page does not exists";
            }
            else
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();
                TempData["Error"] = "Page has been deleted";
            }

            return RedirectToAction("Index");
        }
        //POST /admin /Pages /reoder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] Id)
        {
            int count = 1;
            foreach(var pageId in Id)
            {
                Page page = await context.Pages.FindAsync(pageId);
                page.Sorting = count;
                context.Update(page);   
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }

    }
}
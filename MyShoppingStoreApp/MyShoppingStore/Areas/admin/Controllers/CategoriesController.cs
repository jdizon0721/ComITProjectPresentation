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
    public class CategoriesController : Controller
    {
        private readonly MyShoppingStoreContext context;

        public CategoriesController(MyShoppingStoreContext context)
        {
            this.context = context;
        }
        //Take /admin /categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }

        //Take /admin /categories /create 
        public IActionResult Create() => View();

        //POST /admin /categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;

                var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exists");
                    return View(category);
                }

                context.Add(category);
                await context.SaveChangesAsync();

                TempData["CategorySuccess"] = "Category has been added";

                return RedirectToAction("Index");
            }
            return View(category);
        }
        //Take /admin /categories /Edit 
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        //POST /admin /categories / Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");

                var slug = await context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exists");
                    return View(category);
                }

                context.Update(category);
                await context.SaveChangesAsync();

                TempData["CategorySuccess"] = "The category has been edited";

                return RedirectToAction("Edit", new {id});

            }

            return View(category);
        }

   //Take /admin /categories / delete 
    public async Task<IActionResult> Delete(int id)
    {
        Category category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            TempData["Error"] = "The category does not exists";
        }
        else
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            TempData["Error"] = "The category has been deleted";
        }

        return RedirectToAction("Index");
        }
        //POST /admin /categories /reoder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] Id)
        {
            int count = 1;
            foreach (var categoryId in Id)
            {
                Category category = await context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }
    }
}

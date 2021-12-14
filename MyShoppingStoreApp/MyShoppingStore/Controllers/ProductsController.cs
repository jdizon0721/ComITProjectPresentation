using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoppingStore.Infrastructure;
using MyShoppingStore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyShoppingStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyShoppingStoreContext context;

        public ProductsController(MyShoppingStoreContext context)
        {
            this.context = context;
        }

        //Take /products
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 8;
            var products = context.Products.OrderByDescending(x => x.Id)
                                  .Skip((p - 1) * pageSize)
                                  .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.Totalpages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);

            return View(await products.ToListAsync());
        }

        //take /products/category
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {
            Category category = await context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");

            int pageSize = 8;
            var products = context.Products.OrderByDescending(x => x.Id)
                                  .Where(x => x.CategoryId == category.Id)
                                  .Skip((p - 1) * pageSize)
                                  .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.Totalpages = (int)Math.Ceiling((decimal)context.Products.Where(x => x.CategoryId == category.Id).Count() / pageSize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = categorySlug;

            return View(await products.ToListAsync());
        }
    }
}

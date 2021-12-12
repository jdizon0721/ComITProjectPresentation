using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShoppingStore.Infrastructure;
using MyShoppingStore.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyShoppingStore.Areas.admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly MyShoppingStoreContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductsController(MyShoppingStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        //Take /admin /products
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 8;
            var products = context.Products.OrderByDescending(x => x.Id)
                                  .Include(x => x.Category)
                                  .Skip((p - 1) * pageSize)
                                  .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.Totalpages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);

            return View(await products.ToListAsync());
        }
        //Take /admin /Products /create 
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");
            return View();
        }

        //POST /admin /Products /create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exists");
                    return View(product);
                }
                string imageName = "noimage.png";
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                product.Image = imageName;

                context.Add(product);
                await context.SaveChangesAsync();

                TempData["ProductisCreated"] = "The product has been Created";

                return RedirectToAction("Index");
            }
            return View(product);
        }
        //Take /admin /Products /details 
        public async Task<IActionResult> Details(int id)
        {
            Product product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //Take /admin /Products /Edit 
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            return View(product);
        }
        //POST /admin /Products /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var slug = await context.Products.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exists");
                    return View(product);
                }
               
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");

                    if (!string.Equals(product.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, product.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;

                }

                context.Update(product);
                await context.SaveChangesAsync();

                TempData["ProductisEdited"] = "The product has been Edited";

                return RedirectToAction("Index");
            }
            return View(product);
        }
        //Take /admin /Products / delete 
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Product does not exists";
            }
            else
            {
                if (!string.Equals(product.Image, "noimage.png"))
                { 
           
                string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                string oldImagePath = Path.Combine(uploadsDir, product.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                TempData["Error"] = "Product has been deleted";
            }
            return RedirectToAction("Index");
        }
    }
}
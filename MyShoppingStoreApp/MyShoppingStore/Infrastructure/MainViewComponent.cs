using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShoppingStore.Infrastructure
{
    public class MainViewComponent: ViewComponent
    {
        private readonly MyShoppingStoreContext context;

        public MainViewComponent(MyShoppingStoreContext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages); 
        }

        private Task<List<Page>>GetPagesAsync()
        {
            return context.Pages.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
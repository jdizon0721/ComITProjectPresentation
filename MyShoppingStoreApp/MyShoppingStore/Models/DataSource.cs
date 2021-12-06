using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyShoppingStore.Infrastructure;
using System;
using System.Linq;

namespace MyShoppingStore.Models
{
    public class DataSource
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MyShoppingStoreContext
                (serviceProvider.GetRequiredService<DbContextOptions<MyShoppingStoreContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }
                context.Pages.AddRange(
                new Page
                {
                    Title = "Home",
                    Slug = "home",
                    Content = "home page",
                    Sorting = 0
                },
                new Page
                {
                    Title = "About Us",
                    Slug = "about-us",
                    Content = "about us page",
                    Sorting = 100
                },
                new Page
                {
                    Title = "Services",
                    Slug = "services",
                    Content = "services page",
                    Sorting = 100
                },
                new Page
                {
                    Title = "Contact",
                    Slug = "contact",
                    Content = "contact page",
                    Sorting = 100
                }
                );
                context.SaveChanges();

            }

        }
    }
}

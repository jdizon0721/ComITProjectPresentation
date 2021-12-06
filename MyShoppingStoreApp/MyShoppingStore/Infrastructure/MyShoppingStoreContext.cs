﻿using Microsoft.EntityFrameworkCore;
using MyShoppingStore.Models;

namespace MyShoppingStore.Infrastructure
{
    public class MyShoppingStoreContext: DbContext
    {
        public MyShoppingStoreContext(DbContextOptions<MyShoppingStoreContext>options)
            : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; } 
    }
}

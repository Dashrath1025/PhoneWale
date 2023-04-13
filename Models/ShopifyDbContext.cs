using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public class ShopifyDbContext:IdentityDbContext
    {

        public ShopifyDbContext(DbContextOptions<ShopifyDbContext> options) : base(options) { }

        private readonly ShopifyDbContext _context;

       
        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Product> Products { get; set; }

       

        public DbSet<UserLike> UserLikes { get; set; }

        public DbSet<Recently> Recentlies { get; set; }

        public DbSet<ApplicationUser> applicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
              new Category() { Id = 1, Name = "Electronics" },
            new Category() { Id = 2, Name = "Soap" },
            new Category() { Id = 3, Name = "Cloth" });

            modelBuilder.Entity<SubCategory>().HasData(
                new SubCategory() { SId = 1, Cid = 1, Name = "Mobiles" },
                new SubCategory() { SId = 2, Cid = 1, Name = "Laptop" },
                new SubCategory() { SId = 3, Cid = 4, Name = "Men Cloth" },
                new SubCategory() { SId = 4, Cid = 4, Name = "Women cloth" }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Vivo V27 pro", Qty = 30, Rate = 30000, Profile = "img.jpg", IsActive = true, CatId = 1, subId = 7, Brand = "vivo", Gen = "5g", color = "blue", no_of_sim = 2, os_version = 13, RAM = 8, ROM = 128 },
                new Product() { Id = 2, Name = "Redmi note 12", Qty = 22, Rate = 25000, Profile = "img.jpg", IsActive = true, CatId = 1, subId = 7, Brand = "xiaomi", Gen = "5g", color = "green", no_of_sim = 1, os_version = 12, RAM = 6, ROM = 64 },
                new Product() { Id = 3, Name = "S23 Ultra", Qty = 87, Rate =123000, Profile = "img.jpg", IsActive = true, CatId = 1, subId = 8, Brand = "Samsung", Gen = "5g", color = "gray", no_of_sim = 2, os_version = 13, RAM = 12, ROM = 512 });

            


            modelBuilder.Entity<UserLike>().HasData(
          new UserLike() { Id = 1, PId = 11, UId = "1", PName = "S23 Ultra" });

        }



    }
}

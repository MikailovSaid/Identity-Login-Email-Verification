using EntityFramework.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderDetail> SliderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Instagram> Instagrams { get; set; }
        public DbSet<ExpertImage> ExpertImages { get; set; }
        public DbSet<AboutAdvantage> aboutAdvantages { get; set; }
        public DbSet<ExpertSection> ExpertSections { get; set; }
        public DbSet<Settings> Settings { get; set; }

        //protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Blog>().HasQueryFilter(m => !m.IsDeleted);
        //    base.onModelCreating(modelBuilder);
        //    //modelBuilder.Entity<Settings>().HasData(
        //    //new Settings
        //    //{
        //    //    Id = 1,
        //    //    HomeProductTake = 8,
        //    //    LoadTable = 4
        //    //}
        //    //);
        //}


    }
}

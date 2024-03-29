﻿using Juan.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<Noneed2> ProductColors { get; set; }
        //public DbSet<Noneed1> ProductSizes { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlCategory> BlCategories { get; set; }
        public DbSet<BlogBlCategory> BlogBlCategories { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<ProductColorSize> ProductColorSizes { get; set; }
        public DbSet<Coment> Coments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ComentReply> ComentReplies{ get; set; }

    }
}

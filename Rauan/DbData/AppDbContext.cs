using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rauan.DbData
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Pod_Category> Pod_Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<LateView> LateViews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ProductInfo> ProductInfos { get; set; }
        public DbSet<InProductInfo> InProductInfos { get; set; }

        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        public DbSet<Request> Requests { get; set; }
        public DbSet<Banner> Banners { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandPodCategory> BrandPodCategories { get; set; }

    }
}

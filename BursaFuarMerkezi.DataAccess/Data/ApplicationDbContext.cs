using BursaFuarMerkezi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BursaFuarMerkezi.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<FuarPage> FuarPages { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Unique indexes for bilingual slugs (ignore NULLs)
            modelBuilder.Entity<FuarPage>()
                .HasIndex(x => x.SlugTr)
                .IsUnique()
                .HasFilter("[SlugTr] IS NOT NULL");
            modelBuilder.Entity<FuarPage>()
                .HasIndex(x => x.SlugEn)
                .IsUnique()
                .HasFilter("[SlugEn] IS NOT NULL");
            modelBuilder.Entity<ContentType>().HasData(
                new ContentType { Id = 1, Name = "BLOG" },
                new ContentType { Id = 2, Name = "DUYURULAR" },
                new ContentType { Id = 3, Name = "HABERLER" }
                );


        }
    }
}

using Microsoft.EntityFrameworkCore;
using WebTruyenTranh.Areas.Admin.Models;

namespace WebTruyenTranh.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<AuthorModel> Authors { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<MangaModel> Mangas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorModel>().ToTable("Author");
            modelBuilder.Entity<GenreModel>().ToTable("Genre");
            modelBuilder.Entity<MangaModel>().ToTable("Manga");
        }
    }
}

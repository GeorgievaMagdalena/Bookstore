using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bookstore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Bookstore.Areas.Identity.Data;

namespace Bookstore.Models
{
    public class BookstoreContext : IdentityDbContext<BookstoreUser>
    {
        public BookstoreContext (DbContextOptions<BookstoreContext> options)
            : base(options)
        {
        }

        public DbSet<Bookstore.Models.Author> Author { get; set; } = default!;
        public DbSet<Bookstore.Models.Book>? Book { get; set; } 

        public DbSet<Bookstore.Models.Genre>? Genre { get; set; }

        public DbSet<Bookstore.Models.Review>? Review { get; set; }

        public DbSet<Bookstore.Models.UserBooks>? UserBooks { get; set; }

        public DbSet<Bookstore.Models.BookGenre>? BookGenre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        /*modelBuilder.Entity<Book>()
               .HasOne(p => p.Author)
               .WithMany(p => p.Books)
               .HasForeignKey(p => p.AuthorId);

        modelBuilder.Entity<UserBooks>()
                .HasOne(u => u.Book)
                .WithMany(u => u.UsersBooks)
                .HasForeignKey(u => u.BookId);

        modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.BookId);

        modelBuilder.Entity<BookGenre>()
                .HasOne(b => b.Book)
                .WithMany(b => b.Genres)
                .HasForeignKey(b => b.BookId);

        modelBuilder.Entity<BookGenre>()
                .HasOne(g => g.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(g => g.GenreId);*/
    }
}

using System;
using System.Collections.Generic;
using BooksApi.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Server.Data;

public partial class PostgresContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserBookRelation> UserBookRelation { get; set; }
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("BooksSchema");
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(b => b.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<UserBookRelation>()
             .HasOne(r => r.Book)
             .WithMany(b => b.UserRelations)
             .HasForeignKey(r => r.BookId)
             .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserBookRelation>()
            .HasOne(r => r.User)
            .WithMany(u => u.BookRelations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserBookRelation>()
            .HasKey(r => new { r.UserId, r.BookId});
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

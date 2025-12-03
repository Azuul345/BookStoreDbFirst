using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStoreDbFirst.Models;

public partial class BookStoreContext : DbContext
{
    public BookStoreContext()
    {
    }

    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<BookTitle> BookTitles { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Inventoryinfo> Inventoryinfos { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<StockBalance> StockBalances { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JEGO\\SQLEXPRESS;Initial catalog = BookStore;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Author__70DAFC149B96EFF8");

            entity.ToTable("Author");

            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BookTitle>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__BookTitl__3BF79E03B09800D8");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Language)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.ReleaseDate).HasColumnName("Release Date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Author).WithMany(p => p.BookTitles)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__BookTitle__Autho__3F466844");

            entity.HasOne(d => d.Genre).WithMany(p => p.BookTitles)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__BookTitle__Genre__5BE2A6F2");

            entity.HasOne(d => d.Publisher).WithMany(p => p.BookTitles)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__BookTitle__Publi__59063A47");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8340C136A");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D105343E4889F0").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genres__0385055E927D893A");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.GenreName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Inventoryinfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Inventoryinfo");

            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.StoreName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFBF982815");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Customer__5441852A");

            entity.HasOne(d => d.Isbn13Navigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__ISBN13__5535A963");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__StoreID__5629CD9C");

            entity.HasOne(d => d.StockBalance).WithMany(p => p.Orders)
                .HasForeignKey(d => new { d.StoreId, d.Isbn13 })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_StockBalance");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__4C657E4B82E0D6C7");

            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PublisherName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StockBalance>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn13 }).HasName("PK__StockBal__183D89013AEF2467");

            entity.ToTable("StockBalance");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");

            entity.HasOne(d => d.Isbn13Navigation).WithMany(p => p.StockBalances)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockBala__ISBN1__44FF419A");

            entity.HasOne(d => d.Store).WithMany(p => p.StockBalances)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockBala__Store__440B1D61");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__Stores__3B82F0E1E0FACAF2");

            entity.Property(e => e.StoreId)
                .ValueGeneratedNever()
                .HasColumnName("StoreID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StoreName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

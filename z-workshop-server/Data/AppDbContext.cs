using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using z_workshop_server.Models;

namespace z_workshop_server.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.CommentId).HasMaxLength(32);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasMaxLength(32);
            entity.Property(e => e.ResponseOf).HasMaxLength(32);
            entity.Property(e => e.UserId).HasMaxLength(32);

            entity
                .HasOne(d => d.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_ProductId");

            entity
                .HasOne(d => d.ResponseOfNavigation)
                .WithMany(p => p.InverseResponseOfNavigation)
                .HasForeignKey(d => d.ResponseOf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_ResponseOf");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_UserId");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UK_Customer_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UK_Customer_Phone").IsUnique();

            entity.Property(e => e.CustomerId).HasMaxLength(32);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Fullname).HasMaxLength(255);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.UserId).HasMaxLength(32);

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_UserId");

            entity
                .HasMany(d => d.Products)
                .WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "Cart",
                    r =>
                        r.HasOne<Product>()
                            .WithMany()
                            .HasForeignKey("Product")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_Cart_Product"),
                    l =>
                        l.HasOne<Customer>()
                            .WithMany()
                            .HasForeignKey("Customer")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_Cart_Customer"),
                    j =>
                    {
                        j.HasKey("Customer", "Product");
                        j.ToTable("Cart");
                        j.IndexerProperty<string>("Customer").HasMaxLength(32);
                        j.IndexerProperty<string>("Product").HasMaxLength(32);
                    }
                );

            entity
                .HasMany(d => d.ProductsNavigation)
                .WithMany(p => p.CustomersNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Library",
                    r =>
                        r.HasOne<Product>()
                            .WithMany()
                            .HasForeignKey("ProductId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_Library_ProductId"),
                    l =>
                        l.HasOne<Customer>()
                            .WithMany()
                            .HasForeignKey("CustomerId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_Library_CustomerId"),
                    j =>
                    {
                        j.HasKey("CustomerId", "ProductId");
                        j.ToTable("Library");
                        j.IndexerProperty<string>("CustomerId").HasMaxLength(32);
                        j.IndexerProperty<string>("ProductId").HasMaxLength(32);
                    }
                );
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasIndex(e => e.Email, "UK_Employee_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UK_Employee_Phone").IsUnique();

            entity.Property(e => e.EmployeeId).HasMaxLength(32);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Fullname).HasMaxLength(255);
            entity.Property(e => e.HiredDate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(255);
            entity.Property(e => e.TerminationDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(32);

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_UserId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasMaxLength(32);
            entity.Property(e => e.ApprovedBy).HasMaxLength(32);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Genre).HasMaxLength(255);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.PublisherId).HasMaxLength(32);
            entity.Property(e => e.Type).HasMaxLength(255);

            entity
                .HasOne(d => d.ApprovedByNavigation)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.ApprovedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ApprovedBy");

            entity
                .HasOne(d => d.Publisher)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_PublisherId");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable("Publisher");

            entity.HasIndex(e => e.Email, "UK_Publisher_Email").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(32);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchase");

            entity.Property(e => e.Id).HasMaxLength(32);
            entity.Property(e => e.CloseAt).HasColumnType("datetime");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasMaxLength(32);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.TotalValue).HasColumnType("decimal(19, 4)");

            entity
                .HasOne(d => d.Customer)
                .WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_CustomerId");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => new { e.PurchaseId, e.ProductId });

            entity.ToTable("PurchaseDetail");

            entity.Property(e => e.PurchaseId).HasMaxLength(32);
            entity.Property(e => e.ProductId).HasMaxLength(32);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(19, 4)");

            entity
                .HasOne(d => d.Product)
                .WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetail_ProductId");

            entity
                .HasOne(d => d.Purchase)
                .WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetail_PurchaseId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "idx_User_Username");

            entity.Property(e => e.UserId).HasMaxLength(32);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(32);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

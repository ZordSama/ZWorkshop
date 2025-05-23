﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UK_Customer_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UK_Customer_Phone").IsUnique();

            entity.HasIndex(e => e.UserId, "UK_Customer_UserId").IsUnique();

            entity.Property(e => e.CustomerId).HasMaxLength(64);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Fullname).HasMaxLength(255);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.UserId).HasMaxLength(64);

            entity
                .HasOne(d => d.User)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_UserId");

            entity
                .HasMany(d => d.Products)
                .WithMany(p => p.Customers)
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
                        j.IndexerProperty<string>("CustomerId").HasMaxLength(64);
                        j.IndexerProperty<string>("ProductId").HasMaxLength(64);
                    }
                );
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasIndex(e => e.Email, "UK_Employee_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UK_Employee_Phone").IsUnique();

            entity.HasIndex(e => e.UserId, "UK_Employee_UserId").IsUnique();

            entity.Property(e => e.EmployeeId).HasMaxLength(64);
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
            entity.Property(e => e.UserId).HasMaxLength(64);

            entity
                .HasOne(d => d.User)
                .WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_UserId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasMaxLength(64);
            entity.Property(e => e.ApprovedBy).HasMaxLength(64);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Genre).HasMaxLength(255);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.PublisherId).HasMaxLength(64);
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

            entity.HasIndex(e => e.Name, "UK_Publisher_Name").IsUnique();

            entity.Property(e => e.PublisherId).HasMaxLength(64);
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

            entity.Property(e => e.PurchaseId).HasMaxLength(64);
            entity.Property(e => e.CloseAt).HasColumnType("datetime");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasMaxLength(64);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");

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

            entity.Property(e => e.PurchaseId).HasMaxLength(64);
            entity.Property(e => e.ProductId).HasMaxLength(64);
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

            entity.Property(e => e.UserId).HasMaxLength(64);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(64);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

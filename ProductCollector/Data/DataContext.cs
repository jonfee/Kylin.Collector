﻿using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Td.Kylin.Collector.Entity;

namespace ProductCollector.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            optionsBuilder.UseSqlServer(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //系统模块接口授权
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(p => p.CategoryId).ValueGeneratedNever();
                entity.HasKey(p => p.CategoryId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.ProductID).ValueGeneratedNever();
                entity.HasKey(p => p.ProductID);
            });

            modelBuilder.Entity<ProductSku>(entity =>
            {
                entity.Property(p => p.SkuID).ValueGeneratedNever();
                entity.HasKey(p => p.SkuID);
            });
        }

        public DbSet<Category> Category { get { return Set<Category>(); } }

        public DbSet<Product> Product { get { return Set<Product>(); } }

        public DbSet<ProductSku> ProductSku { get { return Set<ProductSku>(); } }
    }
}

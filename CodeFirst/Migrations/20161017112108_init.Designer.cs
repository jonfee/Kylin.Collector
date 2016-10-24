using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CodeFirst;

namespace CodeFirst.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20161017112108_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kylin.Collector.Entity.Category", b =>
                {
                    b.Property<long>("CategoryId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Icon")
                        .HasColumnType("varchar(300)");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("Layer")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("ParentId");

                    b.Property<string>("Path")
                        .HasColumnType("varchar(500)");

                    b.Property<int>("Source");

                    b.Property<long>("SourceCategoryId");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Kylin.Collector.Entity.Product", b =>
                {
                    b.Property<long>("ProductID");

                    b.Property<long>("BrandID");

                    b.Property<long>("CategoryID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Intro")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("Path")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Pics")
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("Source");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<float>("Weight");

                    b.Property<string>("mainPic")
                        .HasColumnType("varchar(200)");

                    b.HasKey("ProductID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Kylin.Collector.Entity.ProductSku", b =>
                {
                    b.Property<long>("SkuID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(500)");

                    b.Property<long>("ProductID");

                    b.Property<decimal>("SalePrice");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("SkuID");

                    b.ToTable("ProductSku");
                });
        }
    }
}

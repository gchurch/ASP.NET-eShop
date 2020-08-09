﻿// <auto-generated />
using Ganges.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(GangesDbContext))]
    partial class GangesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ganges.ApplicationCore.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Seller")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Plastic",
                            ImageUrl = "toy.png",
                            Price = 50,
                            Quantity = 2,
                            Seller = "Michael",
                            Title = "Toy"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Hard back",
                            ImageUrl = "book.png",
                            Price = 25,
                            Quantity = 4,
                            Seller = "Peter",
                            Title = "Book"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Bright",
                            ImageUrl = "lamp.png",
                            Price = 75,
                            Quantity = 1,
                            Seller = "David",
                            Title = "Lamp"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

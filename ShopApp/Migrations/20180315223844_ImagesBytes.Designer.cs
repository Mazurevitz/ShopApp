﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ShopApp.Models;
using System;

namespace ShopApp.Migrations
{
    [DbContext(typeof(ShopAppContext))]
    [Migration("20180315223844_ImagesBytes")]
    partial class ImagesBytes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ShopApp.Models.Notebook", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GPU");

                    b.Property<byte[]>("ImageBytes");

                    b.Property<string>("Name");

                    b.Property<string>("Processor");

                    b.Property<decimal>("RAM");

                    b.Property<string>("RouteToImage");

                    b.Property<decimal>("ScreenSizeInch");

                    b.HasKey("ID");

                    b.ToTable("Notebook");
                });
#pragma warning restore 612, 618
        }
    }
}

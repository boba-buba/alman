﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using db_client;

#nullable disable

namespace db_client.Migrations
{
    [DbContext(typeof(AlmanContext))]
    partial class AlmanContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("db_client.Activity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ActivityPrice")
                        .HasColumnType("INTEGER");

                    b.HasKey("ActivityId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("db_client.Child", b =>
                {
                    b.Property<int>("ChildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChildContract")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ChildGroup")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChildLastNAme")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChildName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ChildId");

                    b.ToTable("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
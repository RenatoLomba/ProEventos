﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProEventos.API.Data;

namespace ProEventos.API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20211102191048_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("ProEventos.API.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Batch")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUri")
                        .HasColumnType("TEXT");

                    b.Property<int>("PeopleQty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Place")
                        .HasColumnType("TEXT");

                    b.Property<string>("Theme")
                        .HasColumnType("TEXT");

                    b.HasKey("EventId");

                    b.ToTable("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
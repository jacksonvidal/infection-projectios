﻿// <auto-generated />
using System;
using InfectionWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfectionWebApp.Migrations
{
    [DbContext(typeof(ProjectionDataContext))]
    [Migration("20200323045502_InitialCreaate")]
    partial class InitialCreaate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("InfectionWebApp.Models.Projection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Confirmed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("Deaths")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Recovered")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Projections");
                });
#pragma warning restore 612, 618
        }
    }
}
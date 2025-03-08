﻿// <auto-generated />
using System.Collections.Generic;
using EntityFramework.Preferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EntityFramework.Preferences.Migrations
{
    [DbContext(typeof(MigrationDbContext))]
    partial class MigrationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GridSystem.Domain.Grids.Column", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)");

                    b.Property<int>("GridId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GridId");

                    b.ToTable("Column");

                    b.HasDiscriminator().HasValue("Column");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("GridSystem.Domain.Grids.Grid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Grid");
                });

            modelBuilder.Entity("GridSystem.Domain.Grids.Row", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Dictionary<string, string>>("Data")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("GridId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GridId");

                    b.ToTable("Row");
                });

            modelBuilder.Entity("GridSystem.Domain.Grids.NumericColumn", b =>
                {
                    b.HasBaseType("GridSystem.Domain.Grids.Column");

                    b.Property<int>("DecimalPlaces")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("NumericColumn");
                });

            modelBuilder.Entity("GridSystem.Domain.Grids.SingleSelectColumn", b =>
                {
                    b.HasBaseType("GridSystem.Domain.Grids.Column");

                    b.Property<List<string>>("Values")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasDiscriminator().HasValue("SingleSelectColumn");
                });

            modelBuilder.Entity("GridSystem.Domain.Grids.Column", b =>
                {
                    b.HasOne("GridSystem.Domain.Grids.Grid", "Grid")
                        .WithMany("Columns")
                        .HasForeignKey("GridId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grid");
                });

            modelBuilder.Entity("GridSystem.Domain.Grids.Row", b =>
                {
                    b.HasOne("GridSystem.Domain.Grids.Grid", "Grid")
                        .WithMany("Rows")
                        .HasForeignKey("GridId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grid");
                });

            modelBuilder.Entity("GridSystem.Domain.Grids.Grid", b =>
                {
                    b.Navigation("Columns");

                    b.Navigation("Rows");
                });
#pragma warning restore 612, 618
        }
    }
}

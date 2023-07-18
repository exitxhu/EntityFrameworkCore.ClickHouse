﻿// <auto-generated />
using System.Collections.Generic;
using ClickHouse.EntityFrameworkCore.Metadata;
using EntityFrameworkCore.ClickHouse.TestCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    [DbContext(typeof(ClickHouseContext))]
    [Migration("20230718154107_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.8");

            modelBuilder.Entity("EntityFrameworkCore.ClickHouse.TestCases.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Int32");

                    b.Property<byte>("Agreement")
                        .HasColumnType("UInt8");

                    b.Property<List<KycStatusEnum>>("KycStatus")
                        .IsRequired()
                        .HasColumnType("Array(Int32)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("String");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("String");

                    b.Property<int>("Status")
                        .HasColumnType("Int32");

                    b.HasKey("UserId");

                    b.ToTable("User", (string)null);

                    b
                        .HasAnnotation("ClickHouseTableAttribute", "CREATE")
                        .HasAnnotation("PostgreSQLEngine_ClickHouse:Engine", "{\"EngineType\":\"PostgreSQLEngine_ClickHouse:Engine\",\"Table\":\"User\",\"Schema\":\"Accounting\"}");
                });
#pragma warning restore 612, 618
        }
    }
}

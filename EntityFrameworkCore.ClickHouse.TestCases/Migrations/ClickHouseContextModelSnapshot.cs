﻿// <auto-generated />
using System;
using ClickHouse.EntityFrameworkCore.Metadata;
using EntityFrameworkCore.ClickHouse.TestCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    [DbContext(typeof(ClickHouseContext))]
    partial class ClickHouseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("EntityFrameworkCore.ClickHouse.TestCases.Order", b =>
                {
                    b.Property<long>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Int64");

                    b.Property<long>("Amount")
                        .HasColumnType("Int64");

                    b.Property<DateTime>("Date")
                        .HasColumnType("DateTime");

                    b.HasKey("OrderId");

                    b.ToTable("Order", "Order");

                    b
                        .HasAnnotation("ClickHouseTableAttribute", "CREATE_OR_REPLACE")
                        .HasAnnotation("ReplacingMergeTreeEngine_ClickHouse:Engine", "{\"EngineType\":\"ReplacingMergeTreeEngine_ClickHouse:Engine\",\"OrderBy\":null,\"PartitionBy\":\"toYYYYMM(Date)\",\"PrimaryKey\":\"OrderId,Date\",\"SampleBy\":null,\"Settings\":{\"IndexGranularity\":8192,\"IndexGranularityBytes\":10485760,\"MinIndexGranularityBytes\":1024,\"EnableMixedGranularityParts\":false,\"UseMinimalisticPartHeaderInZookeeper\":false,\"MinMergeBytesToUseDirectIo\":10737418240,\"MergeWithTtlTimeout\":\"1.00:00:00\",\"WriteFinalMark\":false,\"MergeMaxBlockSize\":8192,\"StoragePolicy\":null,\"MinBytesForWidePart\":null,\"MinRowsForWidePart\":null,\"MaxPartsInTotal\":null,\"MaxCompressBlockSize\":null,\"MinCompressBlockSize\":null,\"IsDefault\":true}}");
                });
#pragma warning restore 612, 618
        }
    }
}

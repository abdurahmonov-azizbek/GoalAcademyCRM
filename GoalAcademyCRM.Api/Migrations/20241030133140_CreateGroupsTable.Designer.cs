﻿// <auto-generated />
using System;
using GoalAcademyCRM.Api.Brokers.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GoalAcademyCRM.Api.Migrations
{
    [DbContext(typeof(StorageBroker))]
    [Migration("20241030133140_CreateGroupsTable")]
    partial class CreateGroupsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GoalAcademyCRM.Api.Models.Attendances.Attendance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCame")
                        .HasColumnType("boolean");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("GoalAcademyCRM.Api.Models.Courses.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("GoalAcademyCRM.Api.Models.Groups.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Odd")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Book_Hub_Web_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Book_Hub_Web_API.Migrations
{
    [DbContext(typeof(BookHubDBContext))]
    [Migration("20241124052840_DBMTwo")]
    partial class DBMTwo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Book_Hub_Web_API.Models.Books", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AvailableQuantity")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Edition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publication")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("int");

                    b.HasKey("BookId");

                    b.HasIndex("GenreId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Borrowed", b =>
                {
                    b.Property<int>("BorrowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BorrowId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("date");

                    b.Property<string>("BorrowStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("ReturnDeadline")
                        .HasColumnType("date");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BorrowId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Borrowed");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Fines", b =>
                {
                    b.Property<int>("FineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FineId"));

                    b.Property<int>("BorrowId")
                        .HasColumnType("int");

                    b.Property<decimal>("FineAmount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("FinePaidStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FineType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PaidDate")
                        .HasColumnType("date");

                    b.HasKey("FineId");

                    b.HasIndex("BorrowId");

                    b.ToTable("Fines");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Genres", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenreId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.LogUserActivity", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogId"));

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LogId");

                    b.HasIndex("UserId");

                    b.ToTable("LogUserActivity");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Notifications", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<string>("MessageDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("date");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Reservations", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationId"));

                    b.Property<DateTime>("ApplicationTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpectedAvailabilityDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("ReservationExpiryDate")
                        .HasColumnType("date");

                    b.Property<string>("ReservationStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReservationId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("AccountCreatedDate")
                        .HasColumnType("date");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Books", b =>
                {
                    b.HasOne("Book_Hub_Web_API.Models.Genres", "Genres")
                        .WithMany("Books")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genres");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Borrowed", b =>
                {
                    b.HasOne("Book_Hub_Web_API.Models.Books", "Book")
                        .WithMany("Borrowed")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Book_Hub_Web_API.Models.Users", "User")
                        .WithMany("Borrowed")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Fines", b =>
                {
                    b.HasOne("Book_Hub_Web_API.Models.Borrowed", "Borrowed")
                        .WithMany("Fines")
                        .HasForeignKey("BorrowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Borrowed");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.LogUserActivity", b =>
                {
                    b.HasOne("Book_Hub_Web_API.Models.Users", "User")
                        .WithMany("LogUserActivity")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Notifications", b =>
                {
                    b.HasOne("Book_Hub_Web_API.Models.Users", "Users")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Reservations", b =>
                {
                    b.HasOne("Book_Hub_Web_API.Models.Books", "Book")
                        .WithMany("Reservations")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Book_Hub_Web_API.Models.Users", "Users")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Books", b =>
                {
                    b.Navigation("Borrowed");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Borrowed", b =>
                {
                    b.Navigation("Fines");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Genres", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Book_Hub_Web_API.Models.Users", b =>
                {
                    b.Navigation("Borrowed");

                    b.Navigation("LogUserActivity");

                    b.Navigation("Notifications");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}

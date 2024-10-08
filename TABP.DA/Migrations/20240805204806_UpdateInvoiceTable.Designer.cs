﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TABP.DAL;
using TABP.DAL.DbContexts;

#nullable disable

namespace TABP.DAL.Migrations
{
    [DbContext(typeof(TABPDbContext))]
    [Migration("20240805204806_UpdateInvoiceTable")]
    partial class UpdateInvoiceTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TABP.DAL.Entities.Amenity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Amenities");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("BookingDate")
                        .HasColumnType("date");

                    b.Property<string>("BookingStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("CheckInDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("CheckOutDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("float(10)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BookingDate");

                    b.HasIndex("HotelId");

                    b.HasIndex("UserId");

                    b.HasIndex("CheckInDate", "CheckOutDate");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("TABP.DAL.Entities.BookingDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("RoomId");

                    b.ToTable("BookingDetails");
                });

            modelBuilder.Entity("TABP.DAL.Entities.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostOffice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("TABP.DAL.Entities.CityImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Thumbnail")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("CityImages");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Credential", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Credentials");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                            CreatedAt = new DateTime(2024, 8, 5, 23, 48, 6, 54, DateTimeKind.Local).AddTicks(1370),
                            HashedPassword = "$2a$11$.0I3bFzDhORA0SMV8eNIieR1qJoGVWXkQkFbSVqqS6nuBOvBsdrwO",
                            UserId = new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                            Username = "Admin1"
                        },
                        new
                        {
                            Id = new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                            CreatedAt = new DateTime(2024, 8, 5, 23, 48, 6, 54, DateTimeKind.Local).AddTicks(1379),
                            HashedPassword = "$2a$11$tJjrJ/X9lTu5Vxc7T5Rv1uoNxG0QQa0wFhPEYEtvRPraUku1y8WNm",
                            UserId = new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                            Username = "Customer1"
                        });
                });

            modelBuilder.Entity("TABP.DAL.Entities.Hotel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("Rating");

                    b.ToTable("Hotels");
                });

            modelBuilder.Entity("TABP.DAL.Entities.HotelImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Thumbnail")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("HotelImages");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("float(10)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("float(10)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.HasIndex("PaymentDate");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.HasIndex("UserId", "HotelId")
                        .IsUnique();

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxAdults")
                        .HasColumnType("int");

                    b.Property<int>("MaxChildren")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("float(10)");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomType")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.HasIndex("RoomType");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("TABP.DAL.Entities.RoomAmenity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AmenityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AmenityId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomAmenities");
                });

            modelBuilder.Entity("TABP.DAL.Entities.RoomImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Thumbnail")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomImages");
                });

            modelBuilder.Entity("TABP.DAL.Entities.SpecialOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("StartDate", "EndDate");

                    b.ToTable("SpecialOffers");
                });

            modelBuilder.Entity("TABP.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                            Address = "Admin Address",
                            BirthDate = new DateOnly(2001, 9, 22),
                            CreatedAt = new DateTime(2024, 8, 5, 23, 48, 6, 55, DateTimeKind.Local).AddTicks(6411),
                            Email = "admin@example.com",
                            FirstName = "Admin",
                            LastName = "Admin",
                            PhoneNumber = "1234567890",
                            Role = "Admin"
                        },
                        new
                        {
                            Id = new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                            Address = "Customer Address",
                            BirthDate = new DateOnly(1996, 2, 4),
                            CreatedAt = new DateTime(2024, 8, 5, 23, 48, 6, 55, DateTimeKind.Local).AddTicks(6416),
                            Email = "customer@example.com",
                            FirstName = "Customer",
                            LastName = "Customer",
                            PhoneNumber = "123456789",
                            Role = "Customer"
                        });
                });

            modelBuilder.Entity("TABP.DAL.Entities.Booking", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Hotel", "Hotel")
                        .WithMany("Bookings")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TABP.DAL.Entities.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TABP.DAL.Entities.BookingDetail", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Booking", "Booking")
                        .WithMany("BookingDetails")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TABP.DAL.Entities.Room", "Room")
                        .WithMany("BookingDetails")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TABP.DAL.Entities.CityImage", b =>
                {
                    b.HasOne("TABP.DAL.Entities.City", "City")
                        .WithMany("Images")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Credential", b =>
                {
                    b.HasOne("TABP.DAL.Entities.User", "User")
                        .WithOne("Credential")
                        .HasForeignKey("TABP.DAL.Entities.Credential", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Hotel", b =>
                {
                    b.HasOne("TABP.DAL.Entities.City", "City")
                        .WithMany("Hotels")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("TABP.DAL.Entities.HotelImage", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Hotel", "Hotel")
                        .WithMany("Images")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Invoice", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Booking", "Booking")
                        .WithOne("Invoice")
                        .HasForeignKey("TABP.DAL.Entities.Invoice", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Payment", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Booking", "Booking")
                        .WithOne("Payment")
                        .HasForeignKey("TABP.DAL.Entities.Payment", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TABP.DAL.Entities.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Review", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Hotel", "Hotel")
                        .WithMany("Reviews")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TABP.DAL.Entities.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Room", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Hotel", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("TABP.DAL.Entities.RoomAmenity", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Amenity", "Amenity")
                        .WithMany("RoomAmenities")
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TABP.DAL.Entities.Room", "Room")
                        .WithMany("RoomAmenities")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amenity");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TABP.DAL.Entities.RoomImage", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Room", "Room")
                        .WithMany("Images")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TABP.DAL.Entities.SpecialOffer", b =>
                {
                    b.HasOne("TABP.DAL.Entities.Room", "Room")
                        .WithMany("SpecialOffers")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Amenity", b =>
                {
                    b.Navigation("RoomAmenities");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Booking", b =>
                {
                    b.Navigation("BookingDetails");

                    b.Navigation("Invoice");

                    b.Navigation("Payment")
                        .IsRequired();
                });

            modelBuilder.Entity("TABP.DAL.Entities.City", b =>
                {
                    b.Navigation("Hotels");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Hotel", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Images");

                    b.Navigation("Reviews");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("TABP.DAL.Entities.Room", b =>
                {
                    b.Navigation("BookingDetails");

                    b.Navigation("Images");

                    b.Navigation("RoomAmenities");

                    b.Navigation("SpecialOffers");
                });

            modelBuilder.Entity("TABP.DAL.Entities.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Credential")
                        .IsRequired();

                    b.Navigation("Payments");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}

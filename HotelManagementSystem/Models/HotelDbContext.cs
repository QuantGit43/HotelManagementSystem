using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models;

public partial class HotelDbContext : DbContext
{
    public HotelDbContext()
    {
    }

    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingDetail> BookingDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Service> Services { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BkId).HasName("bookings_pkey");

            entity.ToTable("bookings");

            entity.Property(e => e.BkId).HasColumnName("bk_id");
            entity.Property(e => e.BkClId).HasColumnName("bk_cl_id");
            entity.Property(e => e.BkDateIn).HasColumnName("bk_date_in");
            entity.Property(e => e.BkDateOut).HasColumnName("bk_date_out");
            entity.Property(e => e.BkRmId).HasColumnName("bk_rm_id");

            entity.HasOne(d => d.BkCl).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BkClId)
                .HasConstraintName("bookings_bk_cl_id_fkey");

            entity.HasOne(d => d.BkRm).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BkRmId)
                .HasConstraintName("bookings_bk_rm_id_fkey");
        });

        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.HasKey(e => e.BdId).HasName("booking_details_pkey");

            entity.ToTable("booking_details");

            entity.Property(e => e.BdId).HasColumnName("bd_id");
            entity.Property(e => e.BdBkId).HasColumnName("bd_bk_id");
            entity.Property(e => e.BdQuantity).HasColumnName("bd_quantity");
            entity.Property(e => e.BdSrvId).HasColumnName("bd_srv_id");

            entity.HasOne(d => d.BdBk).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.BdBkId)
                .HasConstraintName("booking_details_bd_bk_id_fkey");

            entity.HasOne(d => d.BdSrv).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.BdSrvId)
                .HasConstraintName("booking_details_bd_srv_id_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.CatId).HasColumnName("cat_id");
            entity.Property(e => e.CatName)
                .HasMaxLength(255)
                .HasColumnName("cat_name");
            entity.Property(e => e.CatPrice)
                .HasPrecision(10, 2)
                .HasColumnName("cat_price");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClId).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.Property(e => e.ClId).HasColumnName("cl_id");
            entity.Property(e => e.ClName)
                .HasMaxLength(255)
                .HasColumnName("cl_name");
            entity.Property(e => e.ClPhone)
                .HasMaxLength(50)
                .HasColumnName("cl_phone");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.HtId).HasName("hotels_pkey");

            entity.ToTable("hotels");

            entity.Property(e => e.HtId).HasColumnName("ht_id");
            entity.Property(e => e.HtAddress)
                .HasMaxLength(255)
                .HasColumnName("ht_address");
            entity.Property(e => e.HtName)
                .HasMaxLength(255)
                .HasColumnName("ht_name");
            entity.Property(e => e.HtStars).HasColumnName("ht_stars");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RmId).HasName("rooms_pkey");

            entity.ToTable("rooms");

            entity.Property(e => e.RmId).HasColumnName("rm_id");
            entity.Property(e => e.RmCatId).HasColumnName("rm_cat_id");
            entity.Property(e => e.RmHtId).HasColumnName("rm_ht_id");
            entity.Property(e => e.RmNumber).HasColumnName("rm_number");

            entity.HasOne(d => d.RmCat).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RmCatId)
                .HasConstraintName("rooms_rm_cat_id_fkey");

            entity.HasOne(d => d.RmHt).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RmHtId)
                .HasConstraintName("rooms_rm_ht_id_fkey");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.SrvId).HasName("services_pkey");

            entity.ToTable("services");

            entity.Property(e => e.SrvId).HasColumnName("srv_id");
            entity.Property(e => e.SrvName)
                .HasMaxLength(255)
                .HasColumnName("srv_name");
            entity.Property(e => e.SrvPrice)
                .HasPrecision(10, 2)
                .HasColumnName("srv_price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

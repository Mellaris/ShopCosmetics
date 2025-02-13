using System;
using System.Collections.Generic;
using Cosmetics.Models;
using Microsoft.EntityFrameworkCore;

namespace Cosmetics.Context;

public partial class User9Context : DbContext
{
    public User9Context()
    {
    }

    public User9Context(DbContextOptions<User9Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productstatus> Productstatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=45.67.56.214;Port=5454;Database=user9;Username=user9;Password=X8C8NTnD");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("category_pkey");

            entity.ToTable("category", "salon");

            entity.HasIndex(e => e.Categoryname, "category_categoryname_key").IsUnique();

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(100)
                .HasColumnName("categoryname");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Manufacturerid).HasName("manufacturer_pkey");

            entity.ToTable("manufacturer", "salon");

            entity.HasIndex(e => e.Manufacturername, "manufacturer_manufacturername_key").IsUnique();

            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Manufacturername)
                .HasMaxLength(100)
                .HasColumnName("manufacturername");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("Order_pkey");

            entity.ToTable("Order", "salon");

            entity.HasIndex(e => e.Ordernumber, "Order_ordernumber_key").IsUnique();

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Datenew)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datenew");
            entity.Property(e => e.Orderdeliverydate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderdeliverydate");
            entity.Property(e => e.Ordernumber)
                .HasMaxLength(50)
                .HasColumnName("ordernumber");
            entity.Property(e => e.Orderpickuppoint).HasColumnName("orderpickuppoint");
            entity.Property(e => e.Orderstatus).HasColumnName("orderstatus");
            entity.Property(e => e.Ordertotalcost).HasColumnName("ordertotalcost");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.OrderpickuppointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderpickuppoint)
                .HasConstraintName("Order_orderpickuppoint_fkey");

            entity.HasOne(d => d.OrderstatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderstatus)
                .HasConstraintName("Order_orderstatus_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("Order_userid_fkey");

            entity.HasMany(d => d.Productarticlenumbers).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "Orderproduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("Productarticlenumber")
                        .HasConstraintName("orderproduct_productarticlenumber_fkey"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("Orderid")
                        .HasConstraintName("orderproduct_orderid_fkey"),
                    j =>
                    {
                        j.HasKey("Orderid", "Productarticlenumber").HasName("orderproduct_pkey");
                        j.ToTable("orderproduct", "salon");
                        j.IndexerProperty<int>("Orderid").HasColumnName("orderid");
                        j.IndexerProperty<string>("Productarticlenumber")
                            .HasMaxLength(100)
                            .HasColumnName("productarticlenumber");
                    });
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("orderstatus_pkey");

            entity.ToTable("orderstatus", "salon");

            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Statusname)
                .HasMaxLength(50)
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.Pickuppointid).HasName("pickuppoint_pkey");

            entity.ToTable("pickuppoint", "salon");

            entity.Property(e => e.Pickuppointid).HasColumnName("pickuppointid");
            entity.Property(e => e.Pickuppointaddress).HasColumnName("pickuppointaddress");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productarticlenumber).HasName("product_pkey");

            entity.ToTable("product", "salon");

            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productcategoryid).HasColumnName("productcategoryid");
            entity.Property(e => e.Productcost)
                .HasPrecision(19, 4)
                .HasColumnName("productcost");
            entity.Property(e => e.Productdescription).HasColumnName("productdescription");
            entity.Property(e => e.Productdiscountamount).HasColumnName("productdiscountamount");
            entity.Property(e => e.Productmanufacturerid).HasColumnName("productmanufacturerid");
            entity.Property(e => e.Productname).HasColumnName("productname");
            entity.Property(e => e.Productphoto).HasColumnName("productphoto");
            entity.Property(e => e.Productquantityinstock).HasColumnName("productquantityinstock");
            entity.Property(e => e.Productstatusid).HasColumnName("productstatusid");

            entity.HasOne(d => d.Productcategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productcategoryid)
                .HasConstraintName("product_productcategoryid_fkey");

            entity.HasOne(d => d.Productmanufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productmanufacturerid)
                .HasConstraintName("product_productmanufacturerid_fkey");

            entity.HasOne(d => d.Productstatus).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productstatusid)
                .HasConstraintName("product_productstatusid_fkey");
        });

        modelBuilder.Entity<Productstatus>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("productstatus_pkey");

            entity.ToTable("productstatus", "salon");

            entity.HasIndex(e => e.Statusname, "productstatus_statusname_key").IsUnique();

            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Statusname)
                .HasMaxLength(50)
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role", "salon");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("User_pkey");

            entity.ToTable("User", "salon");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Userlogin).HasColumnName("userlogin");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Userpassword).HasColumnName("userpassword");
            entity.Property(e => e.Userpatronymic)
                .HasMaxLength(100)
                .HasColumnName("userpatronymic");
            entity.Property(e => e.Userrole).HasColumnName("userrole");
            entity.Property(e => e.Usersurname)
                .HasMaxLength(100)
                .HasColumnName("usersurname");

            entity.HasOne(d => d.UserroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userrole)
                .HasConstraintName("User_userrole_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

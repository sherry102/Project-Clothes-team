using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project.Models;

public partial class DataBaseUniPayContext : DbContext
{
    public DataBaseUniPayContext()
    {
    }

    public DataBaseUniPayContext(DbContextOptions<DataBaseUniPayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TableComment> TableComments { get; set; }

    public virtual DbSet<TableCustomerService> TableCustomerServices { get; set; }

    public virtual DbSet<TableCustomization> TableCustomizations { get; set; }

    public virtual DbSet<TableFavorite> TableFavorites { get; set; }

    public virtual DbSet<TableMember> TableMembers { get; set; }

    public virtual DbSet<TableOrder> TableOrders { get; set; }

    public virtual DbSet<TableOrderDetail> TableOrderDetails { get; set; }

    public virtual DbSet<TableProduct> TableProducts { get; set; }

    public virtual DbSet<TableProductsImage> TableProductsImages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=DataBaseUniPay;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TableComment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableComment");

            entity.Property(e => e.CommentCreateDate).HasColumnType("datetime");
            entity.Property(e => e.CommentDepiction).HasMaxLength(500);
            entity.Property(e => e.CommentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("CommentID");
            entity.Property(e => e.CommentImage1).HasMaxLength(500);
            entity.Property(e => e.CommentImage2).HasMaxLength(500);
            entity.Property(e => e.CommentImage3).HasMaxLength(500);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MemberName).HasMaxLength(20);
        });

        modelBuilder.Entity<TableCustomerService>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableCustomerService");

            entity.Property(e => e.CustomerServiceGmtimes)
                .HasColumnType("datetime")
                .HasColumnName("CustomerServiceGMTimes");
            entity.Property(e => e.CustomerServiceId).HasColumnName("CustomerServiceID");
            entity.Property(e => e.CustomerServiceMemberTimes).HasColumnType("datetime");
            entity.Property(e => e.CustomerServiceStatus).HasMaxLength(50);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
        });

        modelBuilder.Entity<TableCustomization>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableCustomization");

            entity.Property(e => e.CustomizationCategory).HasMaxLength(20);
            entity.Property(e => e.CustomizationColor).HasMaxLength(20);
            entity.Property(e => e.CustomizationFont).HasMaxLength(20);
            entity.Property(e => e.CustomizationId)
                .ValueGeneratedOnAdd()
                .HasColumnName("CustomizationID");
            entity.Property(e => e.CustomizationImage).HasMaxLength(500);
            entity.Property(e => e.CustomizationName).HasMaxLength(20);
            entity.Property(e => e.CustomizationSize).HasMaxLength(20);
            entity.Property(e => e.CustomizationText).HasMaxLength(500);
            entity.Property(e => e.CustomizationType).HasMaxLength(20);
        });

        modelBuilder.Entity<TableFavorite>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableFavorite");

            entity.Property(e => e.FavoriteCreateDate).HasColumnType("datetime");
            entity.Property(e => e.FavoriteId)
                .ValueGeneratedOnAdd()
                .HasColumnName("FavoriteID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ProductsId).HasColumnName("ProductsID");
        });

        modelBuilder.Entity<TableMember>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableMember");

            entity.Property(e => e.MemberAccount).HasMaxLength(20);
            entity.Property(e => e.MemberAddress).HasMaxLength(200);
            entity.Property(e => e.MemberCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MemberId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MemberID");
            entity.Property(e => e.MemberName).HasMaxLength(50);
            entity.Property(e => e.MemberPassword).HasMaxLength(20);
            entity.Property(e => e.MemberPhone).HasMaxLength(30);
            entity.Property(e => e.MemberPhoto).HasMaxLength(500);
        });

        modelBuilder.Entity<TableOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableOrder");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderAddress).HasMaxLength(500);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("OrderID");
            entity.Property(e => e.OrderName).HasMaxLength(50);
            entity.Property(e => e.OrderPhone).HasMaxLength(20);
            entity.Property(e => e.OrderStatus).HasMaxLength(20);
        });

        modelBuilder.Entity<TableOrderDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableOrderDetail");

            entity.Property(e => e.CustomizationFont).HasMaxLength(20);
            entity.Property(e => e.CustomizationImage).HasMaxLength(500);
            entity.Property(e => e.CustomizationText).HasMaxLength(500);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderDetailId)
                .ValueGeneratedOnAdd()
                .HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderName).HasMaxLength(20);
            entity.Property(e => e.ProductsColor).HasMaxLength(20);
            entity.Property(e => e.ProductsId).HasColumnName("ProductsID");
            entity.Property(e => e.ProductsName).HasMaxLength(20);
            entity.Property(e => e.ProductsSize).HasMaxLength(20);
        });

        modelBuilder.Entity<TableProduct>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ProductsCategory).HasMaxLength(20);
            entity.Property(e => e.ProductsColor).HasMaxLength(20);
            entity.Property(e => e.ProductsDepiction).HasMaxLength(500);
            entity.Property(e => e.ProductsId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ProductsID");
            entity.Property(e => e.ProductsName).HasMaxLength(50);
            entity.Property(e => e.ProductsSize).HasMaxLength(20);
            entity.Property(e => e.ProductsType).HasMaxLength(20);
        });

        modelBuilder.Entity<TableProductsImage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableProductsImage");

            entity.Property(e => e.ProductsId).HasColumnName("ProductsID");
            entity.Property(e => e.ProductsImageId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ProductsImageID");
            entity.Property(e => e.ProductsImageName).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

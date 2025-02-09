﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project.Models;

public partial class DbuniPayContext : DbContext
{
    public DbuniPayContext()
    {
    }

    public DbuniPayContext(DbContextOptions<DbuniPayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tadvice> Tadvices { get; set; }

    public virtual DbSet<Tcart> Tcarts { get; set; }

    public virtual DbSet<Tchat> Tchats { get; set; }

    public virtual DbSet<Tcomment> Tcomments { get; set; }

    public virtual DbSet<Tcoupon> Tcoupons { get; set; }

    public virtual DbSet<Tcustomization> Tcustomizations { get; set; }

    public virtual DbSet<Tfavorite> Tfavorites { get; set; }

    public virtual DbSet<Tmember> Tmembers { get; set; }

    public virtual DbSet<TmemberCoupon> TmemberCoupons { get; set; }

    public virtual DbSet<Tmessage> Tmessages { get; set; }

    public virtual DbSet<Torder> Torders { get; set; }

    public virtual DbSet<TorderDetail> TorderDetails { get; set; }

    public virtual DbSet<Tpimage> Tpimages { get; set; }

    public virtual DbSet<Tproduct> Tproducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tadvice>(entity =>
        {
            entity.ToTable("TAdvice");

            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.Mid).HasColumnName("MId");
            entity.Property(e => e.Oid).HasColumnName("OId");
            entity.Property(e => e.Question).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Tchat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__TChat__A9FBE626A5779CCB");

            entity.ToTable("TChat");

            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.ChatConnectId).HasColumnName("ChatConnectID");
            entity.Property(e => e.ChatCreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mid).HasColumnName("MID");
        });

        modelBuilder.Entity<Tcart>(entity =>
        {
            entity.ToTable("TCart");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomText0).HasMaxLength(50);
            entity.Property(e => e.CustomText1).HasMaxLength(50);
            entity.Property(e => e.Mid).HasColumnName("MID");
            entity.Property(e => e.Pcategory)
                .HasMaxLength(50)
                .HasColumnName("PCategory");
            entity.Property(e => e.Pcolor)
                .HasMaxLength(50)
                .HasColumnName("PColor");
            entity.Property(e => e.Pcount).HasColumnName("PCount");
            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Pname)
                .HasMaxLength(50)
                .HasColumnName("PName");
            entity.Property(e => e.Pprice).HasColumnName("PPrice");
            entity.Property(e => e.Psize)
                .HasMaxLength(50)
                .HasColumnName("PSize");
            entity.Property(e => e.Ptype)
                .HasMaxLength(50)
                .HasColumnName("PType");
        });

        modelBuilder.Entity<Tcomment>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PK__TComment__E15F4132E21C5029");

            entity.ToTable("TComment");

            entity.Property(e => e.ComId).HasColumnName("ComID");
            entity.Property(e => e.ComCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ComDescription).HasMaxLength(500);
            entity.Property(e => e.ComImage1).HasMaxLength(500);
            entity.Property(e => e.ComImage2).HasMaxLength(500);
            entity.Property(e => e.ComImage3).HasMaxLength(500);
            entity.Property(e => e.Mid).HasColumnName("MID");
            entity.Property(e => e.Mname)
                .HasMaxLength(20)
                .HasColumnName("MName");
        });

        modelBuilder.Entity<Tcoupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TCoupon__384AF1DAADD0377B");

            entity.ToTable("TCoupon");

            entity.Property(e => e.CouponName).HasMaxLength(50);
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.PassWord).HasMaxLength(50);
        });

        modelBuilder.Entity<Tcustomization>(entity =>
        {
            entity.HasKey(e => e.Cid).HasName("PK__TCustomi__C1F8DC59F6095E80");

            entity.ToTable("TCustomization");

            entity.Property(e => e.Cid).HasColumnName("CID");
            entity.Property(e => e.Ccategory)
                .HasMaxLength(20)
                .HasColumnName("CCategory");
            entity.Property(e => e.Ccolor)
                .HasMaxLength(20)
                .HasColumnName("CColor");
            entity.Property(e => e.Cfont)
                .HasMaxLength(20)
                .HasColumnName("CFont");
            entity.Property(e => e.CfontSize).HasColumnName("CFontSize");
            entity.Property(e => e.Cimage)
                .HasMaxLength(500)
                .HasColumnName("CImage");
            entity.Property(e => e.Cname)
                .HasMaxLength(20)
                .HasColumnName("CName");
            entity.Property(e => e.Cprice).HasColumnName("CPrice");
            entity.Property(e => e.Csize)
                .HasMaxLength(20)
                .HasColumnName("CSize");
            entity.Property(e => e.Ctext)
                .HasMaxLength(500)
                .HasColumnName("CText");
            entity.Property(e => e.Ctype)
                .HasMaxLength(20)
                .HasColumnName("CType");
        });

        modelBuilder.Entity<Tfavorite>(entity =>
        {
            entity.HasKey(e => e.Fid).HasName("PK__TFavorit__C1BEA5A25E5FE2F7");

            entity.ToTable("TFavorite");

            entity.Property(e => e.Fid).HasColumnName("FID");
            entity.Property(e => e.FcreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FCreateDate");
            entity.Property(e => e.Mid).HasColumnName("MID");
            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Pname)
                .HasMaxLength(20)
                .HasColumnName("PName");
            entity.Property(e => e.Pphoto)
                .HasMaxLength(500)
                .HasColumnName("PPhoto");
            entity.Property(e => e.Pprice).HasColumnName("PPrice");
        });

        modelBuilder.Entity<Tmember>(entity =>
        {
            entity.HasKey(e => e.Mid).HasName("PK__TMember__C797348ADA665940");

            entity.ToTable("TMember");

            entity.Property(e => e.Mid).HasColumnName("MID");
            entity.Property(e => e.Maccount)
                .HasMaxLength(20)
                .HasColumnName("MAccount");
            entity.Property(e => e.Maddress)
                .HasMaxLength(200)
                .HasColumnName("MAddress");
            entity.Property(e => e.Mbirthday).HasColumnName("MBirthday");
            entity.Property(e => e.McreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("MCreatedDate");
            entity.Property(e => e.Memail)
                .HasMaxLength(200)
                .HasColumnName("MEmail");
            entity.Property(e => e.Mgender).HasColumnName("MGender");
            entity.Property(e => e.MisHided).HasColumnName("MIsHided");
            entity.Property(e => e.Mname)
                .HasMaxLength(50)
                .HasColumnName("MName");
            entity.Property(e => e.Mpassword)
                .HasMaxLength(20)
                .HasColumnName("MPassword");
            entity.Property(e => e.Mpermissions).HasColumnName("MPermissions");
            entity.Property(e => e.Mphone)
                .HasMaxLength(30)
                .HasColumnName("MPhone");
            entity.Property(e => e.Mphoto)
                .HasMaxLength(500)
                .HasColumnName("MPhoto");
            entity.Property(e => e.Mpoints).HasColumnName("MPoints");
        });

        modelBuilder.Entity<TmemberCoupon>(entity =>
        {
            entity.ToTable("TMemberCoupon");

            entity.Property(e => e.Mid).HasColumnName("MId");
        });

        modelBuilder.Entity<Tmessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__TMessage__C87C037CEC4E5ADB");

            entity.ToTable("TMessage");

            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.MessageSendId).HasColumnName("MessageSendID");
            entity.Property(e => e.MessageTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Torder>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK__TOrder__CB394B39606C2D74");

            entity.ToTable("TOrder");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Mid).HasColumnName("MID");
            entity.Property(e => e.Oaddress)
                .HasMaxLength(500)
                .HasColumnName("OAddress");
            entity.Property(e => e.Odate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ODate");
            entity.Property(e => e.Odiscountedprice)
                .HasDefaultValue(0)
                .HasColumnName("ODiscountedprice");
            entity.Property(e => e.Oname)
                .HasMaxLength(50)
                .HasColumnName("OName");
            entity.Property(e => e.Opayment).HasColumnName("OPayment");
            entity.Property(e => e.Ophone)
                .HasMaxLength(20)
                .HasColumnName("OPhone");
            entity.Property(e => e.Ostatus)
                .HasMaxLength(20)
                .HasColumnName("OStatus");
            entity.Property(e => e.OtotalPrice).HasColumnName("OTotalPrice");
        });

        modelBuilder.Entity<TorderDetail>(entity =>
        {
            entity.HasKey(e => e.Odid).HasName("PK__TOrderDe__AD346C153236CBC5");

            entity.ToTable("TOrderDetail");

            entity.Property(e => e.Odid).HasColumnName("ODID");
            entity.Property(e => e.Ccounts).HasColumnName("CCounts");
            entity.Property(e => e.Cfont)
                .HasMaxLength(20)
                .HasDefaultValueSql("((0))")
                .HasColumnName("CFont");
            entity.Property(e => e.CfontSize).HasColumnName("CFontSize");
            entity.Property(e => e.Cid).HasColumnName("CID");
            entity.Property(e => e.Cimage)
                .HasMaxLength(500)
                .HasDefaultValueSql("((0))")
                .HasColumnName("CImage");
            entity.Property(e => e.Csize)
                .HasMaxLength(20)
                .HasColumnName("CSize");
            entity.Property(e => e.Ctext)
                .HasMaxLength(500)
                .HasDefaultValueSql("((0))")
                .HasColumnName("CText");
            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Pcolor)
                .HasMaxLength(20)
                .HasDefaultValueSql("((0))")
                .HasColumnName("PColor");
            entity.Property(e => e.Pcounts).HasColumnName("PCounts");
            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Pimage)
                .HasMaxLength(500)
                .HasDefaultValueSql("((0))")
                .HasColumnName("PImage");
            entity.Property(e => e.Pname)
                .HasMaxLength(20)
                .HasColumnName("PName");
            entity.Property(e => e.Pprice).HasColumnName("PPrice");
            entity.Property(e => e.Psize)
                .HasMaxLength(20)
                .HasDefaultValueSql("((0))")
                .HasColumnName("PSize");
        });

        modelBuilder.Entity<Tpimage>(entity =>
        {
            entity.HasKey(e => e.Piid).HasName("PK__TPImage__5F86BE60FDC837AC");

            entity.ToTable("TPImage");

            entity.Property(e => e.Piid).HasColumnName("PIID");
            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Piname)
                .HasMaxLength(500)
                .HasColumnName("PIName");
        });

        modelBuilder.Entity<Tproduct>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__TProduct__C57755202E108E38");

            entity.ToTable("TProducts");

            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Pcategory)
                .HasMaxLength(20)
                .HasColumnName("PCategory");
            entity.Property(e => e.Pcolor)
                .HasMaxLength(20)
                .HasColumnName("PColor");
            entity.Property(e => e.PcreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("PCreatedDate");
            entity.Property(e => e.Pdescription)
                .HasMaxLength(500)
                .HasColumnName("PDescription");
            entity.Property(e => e.Pinventory).HasColumnName("PInventory");
            entity.Property(e => e.PisHided).HasColumnName("PIsHided");
            entity.Property(e => e.Pname)
                .HasMaxLength(50)
                .HasColumnName("PName");
            entity.Property(e => e.Pphoto)
                .HasMaxLength(500)
                .HasColumnName("PPhoto");
            entity.Property(e => e.Pprice).HasColumnName("PPrice");
            entity.Property(e => e.Psize)
                .HasMaxLength(20)
                .HasColumnName("PSize");
            entity.Property(e => e.Ptype)
                .HasMaxLength(20)
                .HasColumnName("PType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

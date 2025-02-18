using System;
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

    public virtual DbSet<EcpayOrder> EcpayOrders { get; set; }

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

    public virtual DbSet<TorderDetail1> TorderDetails1 { get; set; }

    public virtual DbSet<Tpimage> Tpimages { get; set; }

    public virtual DbSet<Tproduct> Tproducts { get; set; }

    public virtual DbSet<TproductInventory> TproductInventories { get; set; }

    public virtual DbSet<Tstyle> Tstyles { get; set; }

    public virtual DbSet<TstyleImg> TstyleImgs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EcpayOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EcpayOrd__3214EC07E0E365A0");

            entity.Property(e => e.MemberId)
                .HasMaxLength(50)
                .HasColumnName("MemberID");
            entity.Property(e => e.MerchantTradeNo).HasMaxLength(50);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.PaymentTypeChargeFee).HasMaxLength(50);
            entity.Property(e => e.RtnMsg).HasMaxLength(50);
            entity.Property(e => e.TradeDate).HasMaxLength(50);
            entity.Property(e => e.TradeNo).HasMaxLength(50);
        });

        modelBuilder.Entity<Tadvice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TAdvice__3214EC07C412FB99");

            entity.ToTable("TAdvice");

            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.Mid).HasColumnName("MId");
            entity.Property(e => e.Oid).HasColumnName("OId");
            entity.Property(e => e.Question).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Tcart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TCart__3214EC078A35550C");

            entity.ToTable("TCart");

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

        modelBuilder.Entity<Tchat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__TChat__A9FBE626462D8C77");

            entity.ToTable("TChat");

            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.ChatConnectId).HasColumnName("ChatConnectID");
            entity.Property(e => e.ChatCreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mid).HasColumnName("MID");
        });

        modelBuilder.Entity<Tcomment>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PK__TComment__E15F41322FD8D932");

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
            entity.HasKey(e => e.Id).HasName("PK__TCoupon__3214EC074DEC64F3");

            entity.ToTable("TCoupon");

            entity.Property(e => e.CouponName).HasMaxLength(50);
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.PassWord).HasMaxLength(50);
        });

        modelBuilder.Entity<Tcustomization>(entity =>
        {
            entity.HasKey(e => e.Cid).HasName("PK__TCustomi__C1F8DC59A5048A62");

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
            entity.HasKey(e => e.Fid).HasName("PK__TFavorit__C1BEA5A2A75E903D");

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
            entity.HasKey(e => e.Mid).HasName("PK__TMember__C797348AF3285D91");

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
            entity.HasKey(e => e.Id).HasName("PK__TMemberC__3214EC075277EC46");

            entity.ToTable("TMemberCoupon");

            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.Mid).HasColumnName("MID");
        });

        modelBuilder.Entity<Tmessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__TMessage__C87C037C9A1B7FDB");

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
            entity.HasKey(e => e.Oid).HasName("PK__TOrder__CB394B39618503B8");

            entity.ToTable("TOrder");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Mid).HasColumnName("MID");
            entity.Property(e => e.Oaddress)
                .HasMaxLength(500)
                .HasColumnName("OAddress");
            entity.Property(e => e.OcancelDate)
                .HasColumnType("datetime")
                .HasColumnName("OCancelDate");
            entity.Property(e => e.OcancelDescription)
                .HasMaxLength(255)
                .HasColumnName("OCancelDescription");
            entity.Property(e => e.OcancelReason)
                .HasMaxLength(50)
                .HasColumnName("OCancelReason");
            entity.Property(e => e.OcancelStatus)
                .HasMaxLength(10)
                .HasDefaultValue("未取消")
                .HasColumnName("OCancelStatus");
            entity.Property(e => e.Odate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ODate");
            entity.Property(e => e.Odiscountedprice).HasColumnName("ODiscountedprice");
            entity.Property(e => e.Oemail)
                .HasMaxLength(50)
                .HasColumnName("OEmail");
            entity.Property(e => e.Oname)
                .HasMaxLength(50)
                .HasColumnName("OName");
            entity.Property(e => e.Opayment).HasColumnName("OPayment");
            entity.Property(e => e.OpaymentDate).HasColumnName("OPaymentDate");
            entity.Property(e => e.Ophone)
                .HasMaxLength(20)
                .HasColumnName("OPhone");
            entity.Property(e => e.Oprice).HasColumnName("OPrice");
            entity.Property(e => e.Ostatus)
                .HasMaxLength(20)
                .HasColumnName("OStatus");
            entity.Property(e => e.OtotalPrice).HasColumnName("OTotalPrice");
            entity.Property(e => e.OtradeDate)
                .HasMaxLength(50)
                .HasColumnName("OTradeDate");
            entity.Property(e => e.OtradeNo)
                .HasMaxLength(50)
                .HasColumnName("OTradeNo");
        });

        modelBuilder.Entity<TorderDetail>(entity =>
        {
            entity.HasKey(e => e.Odid).HasName("PK__TOrderDe__AD346C15E71E9440");

            entity.ToTable("TOrderDetail");

            entity.Property(e => e.Odid).HasColumnName("ODID");
            entity.Property(e => e.CustomText0).HasMaxLength(50);
            entity.Property(e => e.CustomText1).HasMaxLength(50);
            entity.Property(e => e.Oid).HasColumnName("OID");
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
        });

        modelBuilder.Entity<TorderDetail1>(entity =>
        {
            entity.HasKey(e => e.Odid).HasName("PK__TOrderDe__AD346C1569B4AFC8");

            entity.ToTable("TOrderDetails");

            entity.Property(e => e.Odid).HasColumnName("ODID");
            entity.Property(e => e.CustomText0).HasMaxLength(50);
            entity.Property(e => e.CustomText1).HasMaxLength(50);
            entity.Property(e => e.Oid).HasColumnName("OID");
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
        });

        modelBuilder.Entity<Tpimage>(entity =>
        {
            entity.HasKey(e => e.Piid).HasName("PK__TPImage__5F86BE60F51DFF06");

            entity.ToTable("TPImage");

            entity.Property(e => e.Piid).HasColumnName("PIID");
            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Piname)
                .HasMaxLength(500)
                .HasColumnName("PIName");
        });

        modelBuilder.Entity<Tproduct>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__TProduct__C5775520B4C2CC93");

            entity.ToTable("TProducts");

            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Pcategory)
                .HasMaxLength(20)
                .HasColumnName("PCategory");
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
            entity.Property(e => e.Ptype)
                .HasMaxLength(20)
                .HasColumnName("PType");
        });

        modelBuilder.Entity<TproductInventory>(entity =>
        {
            entity.HasKey(e => e.Piid).HasName("PK__TProduct__5F86BE60DD6AC56F");

            entity.ToTable("TProductInventory");

            entity.Property(e => e.Piid).HasColumnName("PIID");
            entity.Property(e => e.Pcolor)
                .HasMaxLength(20)
                .HasColumnName("PColor");
            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.PlastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("PLastUpdated");
            entity.Property(e => e.Psize)
                .HasMaxLength(20)
                .HasColumnName("PSize");
            entity.Property(e => e.Pstock).HasColumnName("PStock");
        });

        modelBuilder.Entity<Tstyle>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TStyle");

            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Sid).HasColumnName("SID");
        });

        modelBuilder.Entity<TstyleImg>(entity =>
        {
            entity.HasKey(e => e.Sid).HasName("PK__TStyleIm__CA195970BE02B544");

            entity.ToTable("TStyleImgs");

            entity.Property(e => e.Sid).HasColumnName("SID");
            entity.Property(e => e.Simg)
                .HasMaxLength(500)
                .HasColumnName("SImg");
            entity.Property(e => e.SimgCategory)
                .HasMaxLength(50)
                .HasColumnName("SImgCategory");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

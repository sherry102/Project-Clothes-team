CREATE DATABASE [DBUniPay]

USE [DBUniPay]

CREATE TABLE [dbo].[TChat](
 [ChatID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [ChatConnectID] [int] NOT NULL,
 [MID] [int] NOT NULL,
 [ChatCreateTime] [datetime] NOT NULL DEFAULT GETDATE())

CREATE TABLE [dbo].[TComment](
 [ComID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [MID] [int] NOT NULL,
 [MName] [nvarchar](20) NOT NULL,
 [ComDescription] [nvarchar](500) NOT NULL,
 [ComCreateDate] [datetime] NOT NULL DEFAULT GETDATE(),
 [ComImage1] [nvarchar](500) NULL,
 [ComImage2] [nvarchar](500) NULL,
 [ComImage3] [nvarchar](500) NULL)

CREATE TABLE [dbo].[TCoupon](
 [CouponID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [CouponName] [nvarchar](50) NOT NULL,
 [MID] [int] NOT NULL,
 [CouponDiscount] [float] NOT NULL)

CREATE TABLE [dbo].[TCustomization](
 [CID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [CName] [nvarchar](20) NOT NULL,
 [CPrice] [int] NOT NULL,
 [CType] [nvarchar](20) NOT NULL,
 [CCategory] [nvarchar](20) NOT NULL,
 [CSize] [nvarchar](20) NOT NULL,
 [CColor] [nvarchar](20) NOT NULL,
 [CText] [nvarchar](500) NOT NULL,
 [CFont] [nvarchar](20) NOT NULL,
 [CFontSize] [float] NOT NULL,
 [CImage] [nvarchar](500) NOT NULL)

CREATE TABLE [dbo].[TFavorite](
 [FID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [MID] [int] NOT NULL,
 [PID] [int] NOT NULL,
 [PName] [nvarchar](20) NOT NULL,
 [PPhoto] [nvarchar](500) NULL,
 [PPrice] [int] NOT NULL,
 [FCreateDate] [datetime] NOT NULL DEFAULT GETDATE())

CREATE TABLE [dbo].[TMember](
 [MID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [MName] [nvarchar](50) NOT NULL,
 [MGender] [int] NOT NULL,
 [MAccount] [nvarchar](20) NOT NULL,
 [MPassword] [nvarchar](20) NOT NULL,
 [MEmail] [nvarchar](200) NOT NULL,
 [MAddress] [nvarchar](200) NOT NULL,
 [MBirthday] [date]NOT NULL,
 [MPhone] [nvarchar](30) NOT NULL,
 [MPoints] [int] NOT NULL DEFAULT 0,
 [MPermissions] [int] NOT NULL,
 [MCreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
 [MPhoto] [nvarchar](500) NULL,
  [MIsHided] [bit] NOT NULL DEFAULT 0)       

CREATE TABLE [dbo].[TMessage](
 [MessageID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [ChatID] [int] NOT NULL,
 [MessageSendID] [int] NOT NULL,
 [MessageContent] [nvarchar](MAX) NOT NULL,
 [MessageTime] [datetime] NOT NULL DEFAULT GETDATE())

CREATE TABLE [dbo].[TOrder](
 [OID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [OName] [nvarchar](50) NOT NULL,
 [ODiscountedprice] [int] DEFAULT 0,
 [OTotalPrice] [int] NOT NULL,
 [ODate] [datetime] NOT NULL DEFAULT GETDATE(),
 [MID] [int] NOT NULL,
 [OAddress] [nvarchar](500) NOT NULL,
 [OPhone] [nvarchar](20) NOT NULL,
 [OStatus] [nvarchar](20) NOT NULL,
 [OPayment] [bit] NOT NULL)

CREATE TABLE [dbo].[TOrderDetail](
 [ODID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [OID] [int] NOT NULL,
 [PID] [int] NOT NULL,
 [PImage] [nvarchar](500) NOT NULL DEFAULT 0,
 [PName] [nvarchar](20) NOT NULL,
 [PPrice] [int] NOT NULL,
 [PCounts] [int] NOT NULL DEFAULT 0,
 [PSize] [nvarchar](20) NOT NULL DEFAULT 0,
 [PColor] [nvarchar](20) NOT NULL DEFAULT 0,
 [CID] [int] NOT NULL,
 [CImage] [nvarchar](500) NOT NULL DEFAULT 0,
 [CSize] [nvarchar](20) NOT NULL ,
 [CCounts] [int] NOT NULL DEFAULT 0,
 [CText] [nvarchar](500) NOT NULL DEFAULT 0,
 [CFont] [nvarchar](20) NOT NULL DEFAULT 0,
 [CFontSize] [float] NOT NULL DEFAULT 0)

CREATE TABLE [dbo].[TPImage](
 [PIID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [PIName] [nvarchar](500) NOT NULL,
 [PID] [int] NOT NULL)

CREATE TABLE [dbo].[TProducts](
 [PID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
 [PName] [nvarchar](50) NOT NULL,
 [PPrice] [int] NOT NULL,
 [PType] [nvarchar](20)NOT NULL,
 [PCategory] [nvarchar](20) NOT NULL,
 [PSize] [nvarchar](20) NOT NULL,
 [PColor] [nvarchar](20) NOT NULL,
 [PDescription] [nvarchar](500) NOT NULL,
 [PInventory] [int] NOT NULL,
 [PCreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
 [PPhoto] [nvarchar](500) NULL,
 [PIsHided] [bit] NOT NULL DEFAULT 0)
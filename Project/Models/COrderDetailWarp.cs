﻿using System.ComponentModel;

namespace Project.Models
{
    public class COrderDetailWarp
    {
        private TorderDetail _orderdetail;

        public TorderDetail orderdetail 
        {
            get { return _orderdetail; }
            set { _orderdetail = value; }
        }
        public COrderDetailWarp()
        {
            _orderdetail = new TorderDetail();
        }

        [DisplayName("訂單明細編號")]
        public int Odid 
        {
            get { return _orderdetail.Odid; }
            set { _orderdetail.Odid = value; }
        }

        [DisplayName("訂單編號")]
        public int Oid
        {
            get { return _orderdetail.Oid; }
            set { _orderdetail.Oid = value; }
        }

        [DisplayName("會員編號")]
        public int Mid
        {
            get { return _orderdetail.Mid; }
            set { _orderdetail.Mid = value; }
        }

        [DisplayName("商品編號")]
        public int Pid
        {
            get { return _orderdetail.Pid; }
            set { _orderdetail.Pid = value; }
        }
        [DisplayName("客製化商品編號")]
        public int Cid
        {
            get { return _orderdetail.Cid; }
            set { _orderdetail.Cid = value; }
        }
        [DisplayName("訂購人名稱")]
        public string Oname
        {
            get { return _orderdetail.Oname; }
            set { _orderdetail.Oname = value; }
        }
        [DisplayName("商品名稱")]
        public string Pname
        {
            get { return _orderdetail.Pname; }
            set { _orderdetail.Pname = value; }
        }
        [DisplayName("商品價格")]
        public int Pprice
        {
            get { return _orderdetail.Pprice; }
            set { _orderdetail.Pprice = value; }
        }

        [DisplayName("商品尺寸")]
        public string Psize
        {
            get { return _orderdetail.Psize; }
            set { _orderdetail.Psize = value; }
        }
        [DisplayName("商品顏色")]
        public string Pcolor
        {
            get { return _orderdetail.Pcolor; }
            set { _orderdetail.Pcolor = value; }
        }
        [DisplayName("客製化文字")]
        public string Ctext
        {
            get { return _orderdetail.Ctext; }
            set { _orderdetail.Ctext = value; }
        }
        [DisplayName("客製化字體")]
        public string Cfont
        {
            get { return _orderdetail.Cfont; }
            set { _orderdetail.Cfont = value; }
        }
        [DisplayName("客製化字體大小")]
        public double CfontSize
        {
            get { return _orderdetail.CfontSize; }
            set { _orderdetail.CfontSize = value; }
        }
        [DisplayName("客製化圖片")]
        public string Cimage
        {
            get { return _orderdetail.Cimage; }
            set { _orderdetail.Cimage = value; }
        }
        [DisplayName("訂單總價")]
        public int OtotalPrice
        {
            get { return _orderdetail.OtotalPrice; }
            set { _orderdetail.OtotalPrice = value; }
        }
        [DisplayName("訂單明細數量")]
        public int Odcounts
        {
            get { return _orderdetail.Odcounts; }
            set { _orderdetail.Odcounts = value; }
        }
        [DisplayName("商品自取")]
        public bool OpickMyself
        {
            get { return _orderdetail.OpickMyself; }
            set { _orderdetail.OpickMyself = value; }
        }

    }
}

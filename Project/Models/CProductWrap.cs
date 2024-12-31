using System.ComponentModel;

namespace Project.Models
{
    public class CProductWrap
    {
        private Tproduct _product;
        public Tproduct product
        {
            get { return _product; }
            set { _product = value; }
        }
        public CProductWrap()
        {
            _product = new Tproduct();
        }

        public int Pid
        {
            get { return _product.Pid; }
            set { _product.Pid = value; }
        }
        [DisplayName("商品名稱")]
        public string Pname
        {
            get { return _product.Pname; }
            set { _product.Pname = value; }
        }
        [DisplayName("商品價格")]
        public int Pprice
        {
            get { return _product.Pprice; }
            set { _product.Pprice = value; }
        }
        [DisplayName("商品系列")]
        public string Ptype
        {
            get { return _product.Ptype; }
            set { _product.Ptype = value; }
        }
        [DisplayName("商品分類")]
        public string Pcategory
        {
            get { return _product.Pcategory; }
            set { _product.Pcategory = value; }
        }
        [DisplayName("商品尺寸")]
        public string Psize
        {
            get { return _product.Psize; }
            set { _product.Psize = value; }
        }
        [DisplayName("商品顏色")]
        public string Pcolor
        {
            get { return _product.Pcolor; }
            set { _product.Pcolor = value; }
        }
        [DisplayName("照片描述")]
        public string Pdepiction
        {
            get { return _product.Pdepiction; }
            set { _product.Pdepiction = value; }
        }
        [DisplayName("商品庫存")]
        public int Pinventory
        {
            get { return _product.Pinventory; }
            set { _product.Pinventory = value; }
        }

        [DisplayName("上傳日期")]
        public string Pdate
        {
            get { return _product.Pdate; }
            set { _product.Pdate = value; }
        }
        [DisplayName("照片")]
        public string Pimage
        {
            get { return _product.Pimage; }
            set { _product.Pimage = value; }
        }

        public IFormFile photoPath { get; set; }

    }
}

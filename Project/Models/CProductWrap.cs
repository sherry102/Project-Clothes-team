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
        public string Pdescription
		{
            get { return _product.Pdescription; }
            set { _product.Pdescription = value; }
        }
        [DisplayName("商品庫存")]
        public int Pinventory
        {
            get { return _product.Pinventory; }
            set { _product.Pinventory = value; }
        }

        [DisplayName("上傳日期")]
        public DateTime PcreatedDate
        {
            get { return _product.PcreatedDate; }
            set { _product.PcreatedDate = value; }
        }
        [DisplayName("照片")]
        public string Pphoto
        {
            get { return _product.Pphoto; }
            set { _product.Pphoto = value; }
        }
        [DisplayName("照片路徑")]
        public IFormFile photoPath { get; set; }

        [DisplayName("是否隱藏")]
        public bool PisHided
        {
            get { return _product.PisHided; }
            set { _product.PisHided = value; }
        }
    }
}

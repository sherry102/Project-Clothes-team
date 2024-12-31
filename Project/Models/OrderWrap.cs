using System.ComponentModel;

namespace Project.Models
{
    public class OrderWrap
    {
        private Torder _order ;

        public Torder order
        {
            get { return _order; }
            set { _order = value; }
        }

        public OrderWrap()
        {
            _order = new Torder();
        }
        [DisplayName("訂單編號")]
        public int Oid 
        { 
            get { return _order.Oid; }
            set { _order.Oid = value; }
        }
        [DisplayName("訂購人名稱")]
        public string Oname
        {
            get { return _order.Oname; }
            set { _order.Oname = value; }
        }
        [DisplayName("訂單折扣價")]
        public int? Odiscountedprice
        {
            get { return _order.Odiscountedprice; }
            set { _order.Odiscountedprice = value; }
        }
        [DisplayName("訂單總價")]
        public int OtotalPrice
        {
            get { return _order.OtotalPrice; }
            set { _order.OtotalPrice = value; }
        }
        [DisplayName("訂單日期")]
        public DateTime Odate
        {
            get { return _order.Odate; }
            set { _order.Odate = value; }
        }
        [DisplayName("會員編號")]
        public int Mid
        {
            get { return _order.Mid; }
            set { _order.Mid = value; }
        }
        [DisplayName("訂單地址")]
        public string Oaddress
        {
            get { return _order.Oaddress; }
            set { _order.Oaddress = value; }
        }
        [DisplayName("訂單電話")]
        public string Ophone
        {
            get { return _order.Ophone; }
            set { _order.Ophone = value; }
        }
        [DisplayName("訂單狀態")]
        public string Ostatus
        {
            get { return _order.Ostatus; }
            set { _order.Ostatus = value; }
        }
        [DisplayName("付款狀態")]
        public bool Opayment
        {
            get { return _order.Opayment; }
            set { _order.Opayment = value; }
        }
    }
}

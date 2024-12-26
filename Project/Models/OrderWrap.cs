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
        [DisplayName("訂購人")]
        public string Oname
        {
            get { return _order.Oname; }
            set { _order.Oname = value; }
        }
        [DisplayName("折價金額")]
        public int? Odiscountedprice
        {
            get { return _order.Odiscountedprice; }
            set { _order.Odiscountedprice = value; }
        }
        [DisplayName("合計")]
        public int OtotalPrice
        {
            get { return _order.OtotalPrice; }
            set { _order.OtotalPrice = value; }
        }
        [DisplayName("訂購時間")]
        public DateTime Odate
        {
            get { return _order.Odate; }
            set { _order.Odate = value; }
        }
        [DisplayName("商品編號")]
        public int Mid
        {
            get { return _order.Mid; }
            set { _order.Mid = value; }
        }
        [DisplayName("訂購人地址")]
        public string Oaddress
        {
            get { return _order.Oaddress; }
            set { _order.Oaddress = value; }
        }
        [DisplayName("訂購人電話")]
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

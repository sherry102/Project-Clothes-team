namespace Project.DTO
{
    public class OrderDTO
    { 
        public string OName { get; set; }  
        public int OTotalPrice { get; set; }  
        public string Odate { get; set; }  
        public string OAddress { get; set; }  
        public string OPhone { get; set; }  
        public string OEmail { get; set; }
        public string OStatus { get; set; }  
        public bool OPayment { get; set; }  
         
        public int? ODiscountedprice { get; set; }  

        public string ItemName { get; set; }

        public string orderId { get; set; }

        public string MerchantTradeNo { get; set; }

        public string MerchantTradeDate { get; set; }
        public string TradeDesc { get; set; }
        public string ExpireDate { get; set; }
        public string ReturnURL { get; set; }
        public string OrderResultURL { get; set; }
        public string PaymentInfoURL { get; set; }

        public string ClientRedirectURL { get; set; }

        public string MerchantID { get; set; }

        public string IgnorePayment { get; set; }

        public string PaymentType { get; set; }

        public string ChoosePayment { get; set; }

        public string EncryptType { get; set; }
    }
}

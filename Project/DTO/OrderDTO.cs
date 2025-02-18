namespace Project.DTO
{
    public class OrderDTO
    {
        public int OID { get; set; }
        public string OName { get; set; }  
        public int OTotalPrice { get; set; }  
        public string Odate { get; set; }  
        public string OAddress { get; set; }  
        public string OPhone { get; set; }  
        public string OEmail { get; set; }
        public string OStatus { get; set; }  
        public bool OPayment { get; set; }  
         
        public int? ODiscountedprice { get; set; }  
         
    }
     
    public class SearchOrderRequest
    {
        public string Condition { get; set; }  // 查詢條件
        public string OrderNumber { get; set; } // 訂單編號
    }
}

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
         
    }
}

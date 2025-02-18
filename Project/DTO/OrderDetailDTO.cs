namespace Project.DTO
{
    public class OrderDetailDTO
    { 
        public int Oid { get; set; }
        public int PId { get; set; }
        public string PName { get; set; } = null!;
        public int PCount { get; set; }
        public string PSize { get; set; } = null!;
        public string PColor { get; set; } = null!;
        public string? CustomText0 { get; set; }
        public string? CustomText1 { get; set; }
        public string? CustomPhoto0 { get; set; }
        public string? CustomPhoto1 { get; set; }
        public string? Photo0 { get; set; }
        public string? Photo1 { get; set; }
        public int PPrice { get; set; }
        public string Ostatus { get; set; }
    }

    public class OrderDetailRequest
    {
        public int Oid { get; set; }
    }

}

namespace Project.DTO
{
    public class ProductDTO
    {
        public int Pid { get; set; }

        public string Pname { get; set; } = null!;

        public int Pprice { get; set; }

        public string Ptype { get; set; } = null!;

        public string Pcategory { get; set; } = null!;

        public string Psize { get; set; } = null!;

        public string Pcolor { get; set; } = null!;

        public string Pdescription { get; set; } = null!;

        public int Pinventory { get; set; }

        public DateTime PcreatedDate { get; set; }

        public string Pphoto { get; set; }

        public bool PisHided { get; set; }
    }
}

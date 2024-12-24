using Project.Models;

namespace Project.ViewModel
{
    public class IndexViewModel
    {
        public List<Torder> Torder { get; set; }

        public List<TorderDetail> TorderDetail { get; set; }

        public int TotalPrice_Today { get; set; }

        public int TotalPrice_Month { get; set; }

        public int TotalPrice_Year { get; set; }

        public int TotalSales_Today { get; set; }

        public int TotalSales_Month { get; set; }

        public int TotalSales_Year { get; set; }

	}
}

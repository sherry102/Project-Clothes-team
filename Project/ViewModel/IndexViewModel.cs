using Project.Models;

namespace Project.ViewModel
{
    public class IndexViewModel
    {
        public List<Torder> Torder { get; set; }

        public List<TorderDetail> TorderDetail { get; set; }

		public List<Tproduct> Tproduct_none { get; set; }

		public List<Torder> Torder_none { get; set; }

        public List<int?> members_count_Month { get; set; }

		public List<int?> members_count_Year { get; set; }

		public List<int> month_Days { get; set; }

        public List<int> product_inventory { get; set; }

		public List<string> product_name { get; set; }


		public int TotalPrice_Today { get; set; }

        public int TotalPrice_Month { get; set; }

        public int TotalPrice_Year { get; set; }

		public int TotalPrice_Yesterday { get; set; }

		public int TotalPrice_LastMonth { get; set; }

		public int TotalPrice_LastYear { get; set; }

		public int TotalSales_Today { get; set; }

        public int TotalSales_Month { get; set; }

        public int TotalSales_Year { get; set; }

		public int TotalSales_Yesterday { get; set; }

		public int TotalSales_LastMonth { get; set; }

		public int TotalSales_LastYear { get; set; }

		public int CustService_today { get; set; }

		public int CustService_month { get; set; }

		public int CustService_year { get; set; }

		public int CustService_yesterday { get; set; }

		public int CustService_lastmonth { get; set; }

		public int CustService_lastyear { get; set; }

	}
}

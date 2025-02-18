using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Torder
{
    public int Oid { get; set; }

    public string Oname { get; set; } = null!;

    public int? Odiscountedprice { get; set; }

    public int OtotalPrice { get; set; }

    public DateTime Odate { get; set; }

    public int Mid { get; set; }

    public string Oaddress { get; set; } = null!;

    public string Ophone { get; set; } = null!;

    public string Oemail { get; set; } = null!;

    public string Ostatus { get; set; } = null!;

    public bool Opayment { get; set; }

    //public string? CancelReason { get; set; } // 取消原因
    //public string? CancelDescription { get; set; } // 取消說明
    //public DateTime? CancelDate { get; set; } // 取消時間
    //public string CancelStatus { get; set; } = "未取消"; // 取消狀態，預設為 "未取消"
}

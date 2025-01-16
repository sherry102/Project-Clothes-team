using System;
using System.Collections.Generic;

namespace Website.Models;

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

    public string Ostatus { get; set; } = null!;

    public bool Opayment { get; set; }
}

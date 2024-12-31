using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TorderDetail
{
    public int Odid { get; set; }

    public int Oid { get; set; }

    public int Mid { get; set; }

    public int Pid { get; set; }

    public int Cid { get; set; }

    public string Oname { get; set; } = null!;

    public string Pname { get; set; } = null!;

    public int Pprice { get; set; }

    public string Psize { get; set; } = null!;

    public string Pcolor { get; set; } = null!;

    public string Ctext { get; set; } = null!;

    public string Cfont { get; set; } = null!;

    public double CfontSize { get; set; }

    public string Cimage { get; set; } = null!;

    public int OtotalPrice { get; set; }

    public int Odcounts { get; set; }

    public bool OpickMyself { get; set; }

}

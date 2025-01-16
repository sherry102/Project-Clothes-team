using System;
using System.Collections.Generic;

namespace Website.Models;

public partial class TorderDetail
{
    public int Odid { get; set; }

    public int Oid { get; set; }

    public int Pid { get; set; }

    public string Pimage { get; set; } = null!;

    public string Pname { get; set; } = null!;

    public int Pprice { get; set; }

    public int Pcounts { get; set; }

    public string Psize { get; set; } = null!;

    public string Pcolor { get; set; } = null!;

    public int Cid { get; set; }

    public string Cimage { get; set; } = null!;

    public string Csize { get; set; } = null!;

    public int Ccounts { get; set; }

    public string Ctext { get; set; } = null!;

    public string Cfont { get; set; } = null!;

    public double CfontSize { get; set; }
}

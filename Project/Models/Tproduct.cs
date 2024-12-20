using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Tproduct
{
    public int Pid { get; set; }

    public string Pname { get; set; } = null!;

    public int Pprice { get; set; }

    public string Ptype { get; set; } = null!;

    public string Pcategory { get; set; } = null!;

    public string Psize { get; set; } = null!;

    public string Pcolor { get; set; } = null!;

    public string Pdepiction { get; set; } = null!;

    public int Pinventory { get; set; }
}

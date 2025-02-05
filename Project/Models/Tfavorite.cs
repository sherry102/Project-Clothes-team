using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Tfavorite
{
    public int Fid { get; set; }

    public int Mid { get; set; }

    public int Pid { get; set; }

    public string Pname { get; set; } = null!;

    public string? Pphoto { get; set; }

    public int Pprice { get; set; }

    public DateTime FcreateDate { get; set; }
}

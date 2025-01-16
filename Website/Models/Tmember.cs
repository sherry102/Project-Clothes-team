using System;
using System.Collections.Generic;

namespace Website.Models;

public partial class Tmember
{
    public int Mid { get; set; }

    public string Mname { get; set; } = null!;

    public int Mgender { get; set; }

    public string Maccount { get; set; } = null!;

    public string Mpassword { get; set; } = null!;

    public string Memail { get; set; } = null!;

    public string Maddress { get; set; } = null!;

    public DateOnly Mbirthday { get; set; }

    public string Mphone { get; set; } = null!;

    public int Mpoints { get; set; }

    public int Mpermissions { get; set; }

    public DateTime McreatedDate { get; set; }

    public string? Mphoto { get; set; }

    public bool MisHided { get; set; }
}

using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Tcomment
{
    public int ComId { get; set; }

    public int Mid { get; set; }

    public string Mname { get; set; } = null!;

    public int Pid { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime ComCreateDate { get; set; }

    public string? ComImage1 { get; set; }

    public string? ComImage2 { get; set; }
}

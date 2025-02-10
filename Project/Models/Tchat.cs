using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Tchat
{
    public int ChatId { get; set; }

    public int ChatConnectId { get; set; }

    public int Mid { get; set; }

    public DateTime ChatCreateTime { get; set; }
}

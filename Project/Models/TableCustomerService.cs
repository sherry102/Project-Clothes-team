using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableCustomerService
{
    public int? CustomerServiceId { get; set; }

    public int? MemberId { get; set; }

    public string? CustomerServiceTexts { get; set; }

    public DateTime? CustomerServiceMemberTimes { get; set; }

    public DateTime? CustomerServiceGmtimes { get; set; }

    public string? CustomerServiceStatus { get; set; }
}

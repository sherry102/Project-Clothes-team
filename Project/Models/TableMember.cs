using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableMember
{
    public int MemberId { get; set; }

    public string? MemberName { get; set; }

    public int? MemberGender { get; set; }

    public string? MemberAccount { get; set; }

    public string? MemberPassword { get; set; }

    public string? MemberAddress { get; set; }

    public DateOnly? MemberBirthday { get; set; }

    public string? MemberPhone { get; set; }

    public int? MemberPoints { get; set; }

    public double? MemberCoupon { get; set; }

    public int? MemberPermissions { get; set; }

    public DateTime? MemberCreatedDate { get; set; }

    public string? MemberPhoto { get; set; }
}

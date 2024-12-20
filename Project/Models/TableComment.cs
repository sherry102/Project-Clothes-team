using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableComment
{
    public int CommentId { get; set; }

    public int? MemberId { get; set; }

    public string? MemberName { get; set; }

    public string? CommentDepiction { get; set; }

    public DateTime? CommentCreateDate { get; set; }

    public string? CommentImage1 { get; set; }

    public string? CommentImage2 { get; set; }

    public string? CommentImage3 { get; set; }
}

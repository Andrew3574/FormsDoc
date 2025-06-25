using System;
using System.Collections.Generic;

namespace Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? FormId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Text { get; set; }

    public virtual Form? Form { get; set; }

    public virtual User? User { get; set; }
}

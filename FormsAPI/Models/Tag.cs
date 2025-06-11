using System;
using System.Collections.Generic;

namespace Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FormTag> FormTags { get; set; } = new List<FormTag>();
}

using System;
using System.Collections.Generic;

namespace Models;

public partial class Topic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Form> Forms { get; set; } = new List<Form>();
}

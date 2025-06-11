using System;
using System.Collections.Generic;

namespace Models;

public partial class FormTag
{
    public int Id { get; set; }

    public int? FormId { get; set; }

    public int? TagId { get; set; }

    public virtual Form? Form { get; set; }

    public virtual Tag? Tag { get; set; }
}

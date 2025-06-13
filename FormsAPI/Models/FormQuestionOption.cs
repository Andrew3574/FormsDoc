using System;
using System.Collections.Generic;

namespace Models;

public partial class FormQuestionOption
{
    public int Id { get; set; }

    public int? FormQuestionId { get; set; }

    public string OptionValue { get; set; } = null!;

    public virtual FormQuestion? FormQuestion { get; set; }
}

using System;
using System.Collections.Generic;

namespace Models;

public partial class FormAnswer
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? FormId { get; set; }

    public DateTime? AsnweredAt { get; set; }

    public virtual ICollection<CheckboxAnswer> CheckboxAnswers { get; set; } = new List<CheckboxAnswer>();

    public virtual Form? Form { get; set; }

    public virtual ICollection<IntegerAnswer> IntegerAnswers { get; set; } = new List<IntegerAnswer>();

    public virtual ICollection<LongTextAnswer> LongTextAnswers { get; set; } = new List<LongTextAnswer>();

    public virtual ICollection<ShortTextAnswer> ShortTextAnswers { get; set; } = new List<ShortTextAnswer>();

    public virtual User? User { get; set; }
}

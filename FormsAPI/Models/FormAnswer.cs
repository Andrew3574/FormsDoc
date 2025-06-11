using System;
using System.Collections.Generic;

namespace Models;

public partial class FormAnswer
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? FormId { get; set; }

    public virtual Form? Form { get; set; }

    public virtual ICollection<FormQuestionAnswer> FormQuestionAnswers { get; set; } = new List<FormQuestionAnswer>();

    public virtual User? User { get; set; }
}

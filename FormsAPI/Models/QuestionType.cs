using System;
using System.Collections.Generic;

namespace Models;

public partial class QuestionType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FormQuestion> FormQuestions { get; set; } = new List<FormQuestion>();
}

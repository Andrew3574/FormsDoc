using System;
using System.Collections.Generic;

namespace Models;

public partial class LongTextAnswer
{
    public int Id { get; set; }

    public int? AnswerId { get; set; }

    public int? FormQuestionId { get; set; }

    public string Answer { get; set; } = null!;

    public virtual FormAnswer? AnswerNavigation { get; set; }

    public virtual FormQuestion? FormQuestion { get; set; }
}

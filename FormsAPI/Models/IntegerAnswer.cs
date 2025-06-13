using System;
using System.Collections.Generic;

namespace Models;

public partial class IntegerAnswer
{
    public int Id { get; set; }

    public int? AnswerId { get; set; }

    public int? FormQuestionId { get; set; }

    public int Answer { get; set; }

    public virtual FormAnswer? AnswerNavigation { get; set; }

    public virtual FormQuestion? FormQuestion { get; set; }
}

using System;
using System.Collections.Generic;

namespace Models;

public partial class CheckboxAnswer
{
    public int Id { get; set; }

    public int? AnswerId { get; set; }

    public int? FormQuestionId { get; set; }

    public bool? Answer { get; set; }

    public virtual FormAnswer? AnswerNavigation { get; set; }

    public virtual FormQuestion? FormQuestion { get; set; }

}

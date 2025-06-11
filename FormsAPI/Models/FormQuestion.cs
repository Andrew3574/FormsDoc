using System;
using System.Collections.Generic;

namespace Models;

public partial class FormQuestion
{
    public int Id { get; set; }

    public int? FormId { get; set; }

    public int? QuestionTypeId { get; set; }

    public string Question { get; set; } = null!;

    public string? Description { get; set; }

    public bool? DisplayState { get; set; }

    public virtual Form? Form { get; set; }

    public virtual ICollection<FormQuestionAnswer> FormQuestionAnswers { get; set; } = new List<FormQuestionAnswer>();

    public virtual QuestionType? QuestionType { get; set; }
}

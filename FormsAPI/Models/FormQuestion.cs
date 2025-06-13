using Models;
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

    public int Position { get; set; }

    public virtual ICollection<CheckboxAnswer> CheckboxAnswers { get; set; } = new List<CheckboxAnswer>();

    public virtual Form? Form { get; set; }

    public virtual ICollection<FormQuestionOption> FormQuestionOptions { get; set; } = new List<FormQuestionOption>();

    public virtual ICollection<IntegerAnswer> IntegerAnswers { get; set; } = new List<IntegerAnswer>();

    public virtual ICollection<LongTextAnswer> LongTextAnswers { get; set; } = new List<LongTextAnswer>();

    public virtual QuestionType? QuestionType { get; set; }

    public virtual ICollection<ShortTextAnswer> ShortTextAnswers { get; set; } = new List<ShortTextAnswer>();
}

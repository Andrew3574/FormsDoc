using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models;

public partial class Form
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public int? Version { get; set; }

    public int? TopicId { get; set; }

    public FormAccessibility Accessibility { get; set; } = FormAccessibility.@public;

    public virtual ICollection<AccessformUser> AccessformUsers { get; set; } = new List<AccessformUser>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<FormAnswer> FormAnswers { get; set; } = new List<FormAnswer>();

    public virtual ICollection<FormQuestion> FormQuestions { get; set; } = new List<FormQuestion>();

    public virtual ICollection<FormTag> FormTags { get; set; } = new List<FormTag>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual Topic? Topic { get; set; }

    public virtual User? User { get; set; }
}

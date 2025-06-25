using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public UserRole Role { get; set; } = UserRole.user;

    public UserState State { get; set; } = UserState.active;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime Lastlogin { get; set; }

    public virtual ICollection<AccessformUser> AccessformUsers { get; set; } = new List<AccessformUser>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<FormAnswer> FormAnswers { get; set; } = new List<FormAnswer>();

    public virtual ICollection<Form> Forms { get; set; } = new List<Form>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}

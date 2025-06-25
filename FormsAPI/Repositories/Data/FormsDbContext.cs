using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using Models.Enums;
using Npgsql;

namespace Repositories.Data;

public partial class FormsDbContext : DbContext
{
    public FormsDbContext()
    {
    }

    public FormsDbContext(DbContextOptions<FormsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessformUser> AccessformUsers { get; set; }

    public virtual DbSet<CheckboxAnswer> CheckboxAnswers { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Form> Forms { get; set; }

    public virtual DbSet<FormAnswer> FormAnswers { get; set; }

    public virtual DbSet<FormQuestion> FormQuestions { get; set; }

    public virtual DbSet<FormQuestionOption> FormQuestionOptions { get; set; }

    public virtual DbSet<FormTag> FormTags { get; set; }

    public virtual DbSet<IntegerAnswer> IntegerAnswers { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<LongTextAnswer> LongTextAnswers { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<ShortTextAnswer> ShortTextAnswers { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var roleConverter = new EnumToStringConverter<UserRole>();
        var stateConverter = new EnumToStringConverter<UserState>();

        modelBuilder.HasPostgresEnum("accessibility", new[] { "public", "restricted" });
        modelBuilder.HasPostgresEnum("role", new[] { "user", "admin" });
        modelBuilder.HasPostgresEnum("state", new[] { "active", "blocked" });

        modelBuilder.Entity<AccessformUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accessform_users_pkey");

            entity.ToTable("accessform_users");

            entity.HasIndex(e => new { e.FormId, e.UserId }, "accessform_users_form_id_user_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FormId).HasColumnName("form_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Form).WithMany(p => p.AccessformUsers)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("accessform_users_form_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.AccessformUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("accessform_users_user_id_fkey");
        });

        modelBuilder.Entity<CheckboxAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("checkbox_answers_pkey");

            entity.ToTable("checkbox_answers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer)
                .HasDefaultValue(false)
                .HasColumnName("answer");
            entity.Property(e => e.AnswerId).HasColumnName("answer_id");
            entity.Property(e => e.FormQuestionId).HasColumnName("form_question_id");

            entity.HasOne(d => d.AnswerNavigation).WithMany(p => p.CheckboxAnswers)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("checkbox_answers_answer_id_fkey");

            entity.HasOne(d => d.FormQuestion).WithMany(p => p.CheckboxAnswers)
                .HasForeignKey(d => d.FormQuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("checkbox_answers_form_question_id_fkey");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at"); 
            entity.Property(e => e.Text)
                .HasColumnName("text");
            entity.Property(e => e.FormId).HasColumnName("form_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Form).WithMany(p => p.Comments)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_form_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_user_id_fkey");
        });

        modelBuilder.Entity<Form>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("forms_pkey");

            entity.ToTable("forms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(256)
                .HasColumnName("image_url");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .HasColumnName("version");
            entity.Property(f => f.Accessibility)
                .HasColumnName("accessibility")
                .HasColumnType("accessibility");

            entity.HasOne(d => d.Topic).WithMany(p => p.Forms)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("forms_topic_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Forms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("forms_user_id_fkey");
        });

        modelBuilder.Entity<FormAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("form_answers_pkey");

            entity.ToTable("form_answers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AsnweredAt)
                .HasPrecision(0)
                .HasDefaultValueSql("now()")
                .HasColumnName("asnwered_at");
            entity.Property(e => e.FormId).HasColumnName("form_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Form).WithMany(p => p.FormAnswers)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_answers_form_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.FormAnswers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_answers_user_id_fkey");
        });

        modelBuilder.Entity<FormQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("form_questions_pkey");

            entity.ToTable("form_questions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayState).HasColumnName("display_state");
            entity.Property(e => e.FormId).HasColumnName("form_id");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.QuestionTypeId).HasColumnName("question_type_id");

            entity.HasOne(d => d.Form).WithMany(p => p.FormQuestions)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_questions_form_id_fkey");

            entity.HasOne(d => d.QuestionType).WithMany(p => p.FormQuestions)
                .HasForeignKey(d => d.QuestionTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_questions_question_type_id_fkey");
        });

        modelBuilder.Entity<FormQuestionOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("form_question_options_pkey");

            entity.ToTable("form_question_options");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FormQuestionId).HasColumnName("form_question_id");
            entity.Property(e => e.OptionValue).HasColumnName("option_value");

            entity.HasOne(d => d.FormQuestion).WithMany(p => p.FormQuestionOptions)
                .HasForeignKey(d => d.FormQuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_question_options_form_question_id_fkey");
        });

        modelBuilder.Entity<FormTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("formtags_pkey");

            entity.ToTable("form_tags");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FormId).HasColumnName("form_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");

            entity.HasOne(d => d.Form).WithMany(p => p.FormTags)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("formtags_form_id_fkey");

            entity.HasOne(d => d.Tag).WithMany(p => p.FormTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("formtags_tag_id_fkey");
        });

        modelBuilder.Entity<IntegerAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("integer_answers_pkey");

            entity.ToTable("integer_answers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.AnswerId).HasColumnName("answer_id");
            entity.Property(e => e.FormQuestionId).HasColumnName("form_question_id");

            entity.HasOne(d => d.AnswerNavigation).WithMany(p => p.IntegerAnswers)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("integer_answers_answer_id_fkey");

            entity.HasOne(d => d.FormQuestion).WithMany(p => p.IntegerAnswers)
                .HasForeignKey(d => d.FormQuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("integer_answers_form_question_id_fkey");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("likes_pkey");

            entity.ToTable("likes");

            entity.HasIndex(e => new { e.UserId, e.FormId }, "likes_user_id_form_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FormId).HasColumnName("form_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Form).WithMany(p => p.Likes)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("likes_form_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("likes_user_id_fkey");
        });

        modelBuilder.Entity<LongTextAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("long_text_answers_pkey");

            entity.ToTable("long_text_answers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer)
                .HasMaxLength(200)
                .HasColumnName("answer");
            entity.Property(e => e.AnswerId).HasColumnName("answer_id");
            entity.Property(e => e.FormQuestionId).HasColumnName("form_question_id");

            entity.HasOne(d => d.AnswerNavigation).WithMany(p => p.LongTextAnswers)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("long_text_answers_answer_id_fkey");

            entity.HasOne(d => d.FormQuestion).WithMany(p => p.LongTextAnswers)
                .HasForeignKey(d => d.FormQuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("long_text_answers_form_question_id_fkey");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_types_pkey");

            entity.ToTable("question_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ShortTextAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("short_text_answers_pkey");

            entity.ToTable("short_text_answers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer)
                .HasMaxLength(50)
                .HasColumnName("answer");
            entity.Property(e => e.AnswerId).HasColumnName("answer_id");
            entity.Property(e => e.FormQuestionId).HasColumnName("form_question_id");

            entity.HasOne(d => d.AnswerNavigation).WithMany(p => p.ShortTextAnswers)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("short_text_answers_answer_id_fkey");

            entity.HasOne(d => d.FormQuestion).WithMany(p => p.ShortTextAnswers)
                .HasForeignKey(d => d.FormQuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("short_text_answers_form_question_id_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tags_pkey");

            entity.ToTable("tags");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("topics_pkey");

            entity.ToTable("topics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_idx").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(32)
                .HasColumnName("email");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(256)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(256)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("surname");
            entity.Property(e => e.Role)
                .HasColumnName("role")
                .HasColumnType("role")
                .HasDefaultValue(UserRole.user);
            entity.Property(e => e.State)
                .HasColumnName("state")
                .HasColumnType("state")
                .HasDefaultValue(UserState.active);
            entity.Property(e => e.Lastlogin)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamptz(0) with time zone")
                .HasColumnName("lastlogin");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    
}

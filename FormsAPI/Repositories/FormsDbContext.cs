using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories;

public partial class FormsDbContext : DbContext
{
    public FormsDbContext()
    {
    }

    public FormsDbContext(DbContextOptions<FormsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Form> Forms { get; set; }

    public virtual DbSet<FormAnswer> FormAnswers { get; set; }

    public virtual DbSet<FormQuestion> FormQuestions { get; set; }

    public virtual DbSet<FormQuestionAnswer> FormQuestionAnswers { get; set; }

    public virtual DbSet<FormTag> FormTags { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("host=localhost;username=postgres;password=qy5k--zhr8a98L;database=FormsDb;port=5432");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("accessibility", new[] { "public", "restricted" })
            .HasPostgresEnum("role", new[] { "user", "admin" })
            .HasPostgresEnum("state", new[] { "active", "blocked" });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
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
                .HasMaxLength(32)
                .HasColumnName("image_url");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .HasColumnName("version");

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

        modelBuilder.Entity<FormQuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("form_question_answers_pkey");

            entity.ToTable("form_question_answers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.AnswerId).HasColumnName("answer_id");
            entity.Property(e => e.FormQuestionId).HasColumnName("form_question_id");

            entity.HasOne(d => d.AnswerNavigation).WithMany(p => p.FormQuestionAnswers)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_question_answers_answer_id_fkey");

            entity.HasOne(d => d.FormQuestion).WithMany(p => p.FormQuestionAnswers)
                .HasForeignKey(d => d.FormQuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_question_answers_form_question_id_fkey");
        });

        modelBuilder.Entity<FormTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fromtags_pkey");

            entity.ToTable("form_tags");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('fromtags_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.FormId).HasColumnName("form_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");

            entity.HasOne(d => d.Form).WithMany(p => p.FormTags)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fromtags_form_id_fkey");

            entity.HasOne(d => d.Tag).WithMany(p => p.FormTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fromtags_tag_id_fkey");
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

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_types_pkey");

            entity.ToTable("question_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
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
                .HasMaxLength(32)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(32)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

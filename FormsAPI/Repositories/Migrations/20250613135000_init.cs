using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:accessibility", "public,restricted")
                .Annotation("Npgsql:Enum:role", "user,admin")
                .Annotation("Npgsql:Enum:state", "active,blocked");

            migrationBuilder.CreateTable(
                name: "question_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("question_types_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tags_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("topics_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    surname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    passwordhash = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    image_url = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "forms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true, defaultValueSql: "now()"),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    image_url = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    version = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    topic_id = table.Column<int>(type: "integer", nullable: true),
                    Accessibility = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("forms_pkey", x => x.id);
                    table.ForeignKey(
                        name: "forms_topic_id_fkey",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "forms_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accessform_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    form_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("accessform_users_pkey", x => x.id);
                    table.ForeignKey(
                        name: "accessform_users_form_id_fkey",
                        column: x => x.form_id,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "accessform_users_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    form_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("comments_pkey", x => x.id);
                    table.ForeignKey(
                        name: "comments_form_id_fkey",
                        column: x => x.form_id,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "comments_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    form_id = table.Column<int>(type: "integer", nullable: true),
                    asnwered_at = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("form_answers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "form_answers_form_id_fkey",
                        column: x => x.form_id,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "form_answers_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    form_id = table.Column<int>(type: "integer", nullable: true),
                    question_type_id = table.Column<int>(type: "integer", nullable: true),
                    question = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    display_state = table.Column<bool>(type: "boolean", nullable: true),
                    position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("form_questions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "form_questions_form_id_fkey",
                        column: x => x.form_id,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "form_questions_question_type_id_fkey",
                        column: x => x.question_type_id,
                        principalTable: "question_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('fromtags_id_seq'::regclass)"),
                    form_id = table.Column<int>(type: "integer", nullable: true),
                    tag_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("fromtags_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fromtags_form_id_fkey",
                        column: x => x.form_id,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fromtags_tag_id_fkey",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    form_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("likes_pkey", x => x.id);
                    table.ForeignKey(
                        name: "likes_form_id_fkey",
                        column: x => x.form_id,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "likes_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "checkbox_answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    answer_id = table.Column<int>(type: "integer", nullable: true),
                    form_question_id = table.Column<int>(type: "integer", nullable: true),
                    answer = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("checkbox_answers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "checkbox_answers_answer_id_fkey",
                        column: x => x.answer_id,
                        principalTable: "form_answers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "checkbox_answers_form_question_id_fkey",
                        column: x => x.form_question_id,
                        principalTable: "form_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_question_options",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    form_question_id = table.Column<int>(type: "integer", nullable: true),
                    option_value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("form_question_options_pkey", x => x.id);
                    table.ForeignKey(
                        name: "form_question_options_form_question_id_fkey",
                        column: x => x.form_question_id,
                        principalTable: "form_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "integer_answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    answer_id = table.Column<int>(type: "integer", nullable: true),
                    form_question_id = table.Column<int>(type: "integer", nullable: true),
                    answer = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("integer_answers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "integer_answers_answer_id_fkey",
                        column: x => x.answer_id,
                        principalTable: "form_answers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "integer_answers_form_question_id_fkey",
                        column: x => x.form_question_id,
                        principalTable: "form_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "long_text_answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    answer_id = table.Column<int>(type: "integer", nullable: true),
                    form_question_id = table.Column<int>(type: "integer", nullable: true),
                    answer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("long_text_answers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "long_text_answers_answer_id_fkey",
                        column: x => x.answer_id,
                        principalTable: "form_answers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "long_text_answers_form_question_id_fkey",
                        column: x => x.form_question_id,
                        principalTable: "form_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "short_text_answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    answer_id = table.Column<int>(type: "integer", nullable: true),
                    form_question_id = table.Column<int>(type: "integer", nullable: true),
                    answer = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("short_text_answers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "short_text_answers_answer_id_fkey",
                        column: x => x.answer_id,
                        principalTable: "form_answers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "short_text_answers_form_question_id_fkey",
                        column: x => x.form_question_id,
                        principalTable: "form_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "accessform_users_form_id_user_id_key",
                table: "accessform_users",
                columns: new[] { "form_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accessform_users_user_id",
                table: "accessform_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkbox_answers_answer_id",
                table: "checkbox_answers",
                column: "answer_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkbox_answers_form_question_id",
                table: "checkbox_answers",
                column: "form_question_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_form_id",
                table: "comments",
                column: "form_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_answers_form_id",
                table: "form_answers",
                column: "form_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_answers_user_id",
                table: "form_answers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_question_options_form_question_id",
                table: "form_question_options",
                column: "form_question_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_questions_form_id",
                table: "form_questions",
                column: "form_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_questions_question_type_id",
                table: "form_questions",
                column: "question_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_tags_form_id",
                table: "form_tags",
                column: "form_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_tags_tag_id",
                table: "form_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_forms_topic_id",
                table: "forms",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_forms_user_id",
                table: "forms",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_integer_answers_answer_id",
                table: "integer_answers",
                column: "answer_id");

            migrationBuilder.CreateIndex(
                name: "IX_integer_answers_form_question_id",
                table: "integer_answers",
                column: "form_question_id");

            migrationBuilder.CreateIndex(
                name: "IX_likes_form_id",
                table: "likes",
                column: "form_id");

            migrationBuilder.CreateIndex(
                name: "likes_user_id_form_id_key",
                table: "likes",
                columns: new[] { "user_id", "form_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_long_text_answers_answer_id",
                table: "long_text_answers",
                column: "answer_id");

            migrationBuilder.CreateIndex(
                name: "IX_long_text_answers_form_question_id",
                table: "long_text_answers",
                column: "form_question_id");

            migrationBuilder.CreateIndex(
                name: "IX_short_text_answers_answer_id",
                table: "short_text_answers",
                column: "answer_id");

            migrationBuilder.CreateIndex(
                name: "IX_short_text_answers_form_question_id",
                table: "short_text_answers",
                column: "form_question_id");

            migrationBuilder.CreateIndex(
                name: "users_email_idx",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accessform_users");

            migrationBuilder.DropTable(
                name: "checkbox_answers");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "form_question_options");

            migrationBuilder.DropTable(
                name: "form_tags");

            migrationBuilder.DropTable(
                name: "integer_answers");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "long_text_answers");

            migrationBuilder.DropTable(
                name: "short_text_answers");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "form_answers");

            migrationBuilder.DropTable(
                name: "form_questions");

            migrationBuilder.DropTable(
                name: "forms");

            migrationBuilder.DropTable(
                name: "question_types");

            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

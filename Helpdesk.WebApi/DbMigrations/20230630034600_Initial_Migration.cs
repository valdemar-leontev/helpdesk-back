using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Helpdesk.WebApi.DbMigrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "business");

            migrationBuilder.EnsureSchema(
                name: "dictionaries");

            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "position",
                schema: "dictionaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "question_type",
                schema: "dictionaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "requirement_category_type",
                schema: "dictionaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_category_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "requirement_state",
                schema: "dictionaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_state", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "requirement_template",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    hasrequirementcategory = table.Column<bool>(name: "has_requirement_category", type: "boolean", nullable: false),
                    creationdate = table.Column<DateTimeOffset>(name: "creation_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTimeOffset>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_template", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "dictionaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subdivision",
                schema: "dictionaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subdivision", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "requirement_category",
                schema: "dictionaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    hasagreement = table.Column<bool>(name: "has_agreement", type: "boolean", nullable: false),
                    requirementcategorytypeid = table.Column<int>(name: "requirement_category_type_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_category_requirement_category_type_data_model_r",
                        column: x => x.requirementcategorytypeid,
                        principalSchema: "dictionaries",
                        principalTable: "requirement_category_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "question",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    isrequired = table.Column<bool>(name: "is_required", type: "boolean", nullable: false),
                    questiontypeid = table.Column<int>(name: "question_type_id", type: "integer", nullable: false),
                    requirementtemplateid = table.Column<int>(name: "requirement_template_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question", x => x.id);
                    table.ForeignKey(
                        name: "fk_question_question_type_question_type_id",
                        column: x => x.questiontypeid,
                        principalSchema: "dictionaries",
                        principalTable: "question_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_question_requirement_template_requirement_template_id",
                        column: x => x.requirementtemplateid,
                        principalSchema: "business",
                        principalTable: "requirement_template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    password = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    roleid = table.Column<int>(name: "role_id", type: "integer", nullable: false),
                    objectsid = table.Column<string>(name: "object_sid", type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role_role_id",
                        column: x => x.roleid,
                        principalSchema: "dictionaries",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subdivision_link_subdivision",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subdivisionid = table.Column<int>(name: "subdivision_id", type: "integer", nullable: false),
                    subdivisionparentid = table.Column<int>(name: "subdivision_parent_id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subdivision_link_subdivision", x => x.id);
                    table.ForeignKey(
                        name: "fk_subdivision_link_subdivision_subdivision_subdivision_id",
                        column: x => x.subdivisionid,
                        principalSchema: "dictionaries",
                        principalTable: "subdivision",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_subdivision_link_subdivision_subdivision_subdivision_parent",
                        column: x => x.subdivisionparentid,
                        principalSchema: "dictionaries",
                        principalTable: "subdivision",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "variant",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    questionid = table.Column<int>(name: "question_id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_variant", x => x.id);
                    table.ForeignKey(
                        name: "fk_variant_question_question_id",
                        column: x => x.questionid,
                        principalSchema: "business",
                        principalTable: "question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "file",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    uploaduserid = table.Column<int>(name: "upload_user_id", type: "integer", nullable: true),
                    creationdate = table.Column<DateTimeOffset>(name: "creation_date", type: "timestamp with time zone", nullable: false),
                    hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_user_upload_user_id",
                        column: x => x.uploaduserid,
                        principalSchema: "admin",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recipientuserid = table.Column<int>(name: "recipient_user_id", type: "integer", nullable: false),
                    message = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    isread = table.Column<bool>(name: "is_read", type: "boolean", nullable: false),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false),
                    creationdate = table.Column<DateTimeOffset>(name: "creation_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_user_recipient_user_id",
                        column: x => x.recipientuserid,
                        principalSchema: "admin",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(name: "first_name", type: "character varying(64)", maxLength: 64, nullable: true),
                    lastname = table.Column<string>(name: "last_name", type: "character varying(64)", maxLength: 64, nullable: true),
                    positionid = table.Column<int>(name: "position_id", type: "integer", nullable: true),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile", x => x.id);
                    table.ForeignKey(
                        name: "fk_profile_position_data_model_position_id",
                        column: x => x.positionid,
                        principalSchema: "dictionaries",
                        principalTable: "position",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_profile_user_user_id",
                        column: x => x.userid,
                        principalSchema: "admin",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_session",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    refreshtoken = table.Column<string>(name: "refresh_token", type: "character varying(96)", maxLength: 96, nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    logindate = table.Column<DateTimeOffset>(name: "login_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_session", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_session_user_user_id",
                        column: x => x.userid,
                        principalSchema: "admin",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_link_subdivision",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    profileid = table.Column<int>(name: "profile_id", type: "integer", nullable: false),
                    subdivisionid = table.Column<int>(name: "subdivision_id", type: "integer", nullable: false),
                    ishead = table.Column<bool>(name: "is_head", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile_link_subdivision", x => x.id);
                    table.ForeignKey(
                        name: "fk_profile_link_subdivision_profile_profile_id",
                        column: x => x.profileid,
                        principalSchema: "business",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_profile_link_subdivision_subdivision_subdivision_id",
                        column: x => x.subdivisionid,
                        principalSchema: "dictionaries",
                        principalTable: "subdivision",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirement",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    requirementtemplateid = table.Column<int>(name: "requirement_template_id", type: "integer", nullable: false),
                    requirementcategoryid = table.Column<int>(name: "requirement_category_id", type: "integer", nullable: true),
                    profileid = table.Column<int>(name: "profile_id", type: "integer", nullable: false),
                    requirementstateid = table.Column<int>(name: "requirement_state_id", type: "integer", nullable: false),
                    outgoingnumber = table.Column<int>(name: "outgoing_number", type: "integer", nullable: false),
                    creationdate = table.Column<DateTimeOffset>(name: "creation_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_profile_profile_id",
                        column: x => x.profileid,
                        principalSchema: "business",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_requirement_category_requirement_category_id",
                        column: x => x.requirementcategoryid,
                        principalSchema: "dictionaries",
                        principalTable: "requirement_category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_requirement_requirement_state_requirement_state_id",
                        column: x => x.requirementstateid,
                        principalSchema: "dictionaries",
                        principalTable: "requirement_state",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_requirement_template_requirement_template_id",
                        column: x => x.requirementtemplateid,
                        principalSchema: "business",
                        principalTable: "requirement_template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirement_category_link_profile",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    requirementcategoryid = table.Column<int>(name: "requirement_category_id", type: "integer", nullable: false),
                    profileid = table.Column<int>(name: "profile_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_category_link_profile", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_category_link_profile_profile_profile_id",
                        column: x => x.profileid,
                        principalSchema: "business",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_category_link_profile_requirement_category_requ",
                        column: x => x.requirementcategoryid,
                        principalSchema: "dictionaries",
                        principalTable: "requirement_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirement_comment",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    requirementid = table.Column<int>(name: "requirement_id", type: "integer", nullable: false),
                    senderprofileid = table.Column<int>(name: "sender_profile_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_comment", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_comment_profile_profile_id",
                        column: x => x.senderprofileid,
                        principalSchema: "business",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_comment_requirement_requirement_id",
                        column: x => x.requirementid,
                        principalSchema: "business",
                        principalTable: "requirement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirement_link_file",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    requirementid = table.Column<int>(name: "requirement_id", type: "integer", nullable: false),
                    fileid = table.Column<int>(name: "file_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_link_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_link_file_file_data_model_file_id",
                        column: x => x.fileid,
                        principalSchema: "business",
                        principalTable: "file",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_link_file_requirement_requirement_id",
                        column: x => x.requirementid,
                        principalSchema: "business",
                        principalTable: "requirement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirement_link_notification",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    notificationid = table.Column<int>(name: "notification_id", type: "integer", nullable: false),
                    requirementid = table.Column<int>(name: "requirement_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_link_notification", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_link_notification_notification_notification_id",
                        column: x => x.notificationid,
                        principalSchema: "business",
                        principalTable: "notification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_link_notification_requirement_requirement_id",
                        column: x => x.requirementid,
                        principalSchema: "business",
                        principalTable: "requirement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirement_link_profile",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    requirementid = table.Column<int>(name: "requirement_id", type: "integer", nullable: false),
                    profileid = table.Column<int>(name: "profile_id", type: "integer", nullable: false),
                    isarchive = table.Column<bool>(name: "is_archive", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_link_profile", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_link_profile_profile_profile_id",
                        column: x => x.profileid,
                        principalSchema: "business",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_link_profile_requirement_requirement_id",
                        column: x => x.requirementid,
                        principalSchema: "business",
                        principalTable: "requirement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirement_stage",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stateid = table.Column<int>(name: "state_id", type: "integer", nullable: false),
                    creationdate = table.Column<DateTimeOffset>(name: "creation_date", type: "timestamp with time zone", nullable: false),
                    requirementid = table.Column<int>(name: "requirement_id", type: "integer", nullable: false),
                    profileid = table.Column<int>(name: "profile_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_stage", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_stage_profile_profile_id",
                        column: x => x.profileid,
                        principalSchema: "business",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_stage_requirement_data_model_requirement_id",
                        column: x => x.requirementid,
                        principalSchema: "business",
                        principalTable: "requirement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_stage_requirement_state_state_id",
                        column: x => x.stateid,
                        principalSchema: "dictionaries",
                        principalTable: "requirement_state",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_answer",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    profileid = table.Column<int>(name: "profile_id", type: "integer", nullable: false),
                    questionid = table.Column<int>(name: "question_id", type: "integer", nullable: false),
                    requirementid = table.Column<int>(name: "requirement_id", type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    variantid = table.Column<int>(name: "variant_id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_answer", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_answer_profile_profile_id",
                        column: x => x.profileid,
                        principalSchema: "business",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_answer_question_question_id",
                        column: x => x.questionid,
                        principalSchema: "business",
                        principalTable: "question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_answer_requirement_data_model_requirement_id",
                        column: x => x.requirementid,
                        principalSchema: "business",
                        principalTable: "requirement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirement_stage_link_requirement_comment",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    requirementstageid = table.Column<int>(name: "requirement_stage_id", type: "integer", nullable: false),
                    requirementcommentid = table.Column<int>(name: "requirement_comment_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_stage_link_requirement_comment", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_stage_link_requirement_comment_requirement_comm",
                        column: x => x.requirementcommentid,
                        principalSchema: "business",
                        principalTable: "requirement_comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirement_stage_link_requirement_comment_requirement_stag",
                        column: x => x.requirementstageid,
                        principalSchema: "business",
                        principalTable: "requirement_stage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dictionaries",
                table: "position",
                columns: new[] { "id", "description" },
                values: new object[,]
                {
                    { 1, "Генеральный директор" },
                    { 2, "Руководитель отдела" },
                    { 3, "Руководитель проекта" },
                    { 4, "Разработчик" },
                    { 5, "Секретарь" }
                });

            migrationBuilder.InsertData(
                schema: "dictionaries",
                table: "question_type",
                columns: new[] { "id", "description" },
                values: new object[,]
                {
                    { 1, "Простой текстовый вопрос" },
                    { 2, "Вопрос с одним вариантом" },
                    { 3, "Вопрос с множеством вариантов" },
                    { 4, "Вопрос с выпадающим списком" },
                    { 5, "Развернутый ответ" }
                });

            migrationBuilder.InsertData(
                schema: "dictionaries",
                table: "requirement_category_type",
                columns: new[] { "id", "description" },
                values: new object[,]
                {
                    { 1, "Интернет" },
                    { 2, "Компьютерная техника" },
                    { 3, "Корпоративная почта" },
                    { 4, "Мобильная связь" },
                    { 5, "Мобильные устройства" },
                    { 6, "Программное обеспечение" },
                    { 7, "Сетевое оборудование" },
                    { 8, "Электронная подпись" },
                    { 9, "IP-Телефония" }
                });

            migrationBuilder.InsertData(
                schema: "dictionaries",
                table: "requirement_state",
                columns: new[] { "id", "description" },
                values: new object[,]
                {
                    { 1, "Создана" },
                    { 2, "В рассмотрении" },
                    { 3, "Согласована" },
                    { 4, "В исполнении" },
                    { 5, "Отказано" },
                    { 6, "Закрыта" },
                    { 7, "Выполнена" },
                    { 8, "Переназначено" }
                });

            migrationBuilder.InsertData(
                schema: "business",
                table: "requirement_template",
                columns: new[] { "id", "creation_date", "description", "has_requirement_category", "name", "update_date" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2023, 6, 30, 3, 46, 0, 305, DateTimeKind.Unspecified).AddTicks(8588), new TimeSpan(0, 0, 0, 0, 0)), "Эта заявка имеет своей целью выявление и решение проблем в сфере IT обеспечения предприятия.", true, "Заявка по IT обеспечению", new DateTimeOffset(new DateTime(2023, 6, 30, 3, 46, 0, 305, DateTimeKind.Unspecified).AddTicks(8588), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "dictionaries",
                table: "role",
                columns: new[] { "id", "code", "description" },
                values: new object[,]
                {
                    { 1, "USER", "Пользователь" },
                    { 2, "ADMIN", "Администратор" },
                    { 3, "API", "Api-клиент" }
                });

            migrationBuilder.InsertData(
                schema: "dictionaries",
                table: "subdivision",
                columns: new[] { "id", "description" },
                values: new object[,]
                {
                    { 1, "Helpdesk International" },
                    { 2, "Helpdesk IT" }
                });

            migrationBuilder.InsertData(
                schema: "business",
                table: "question",
                columns: new[] { "id", "description", "is_required", "question_type_id", "requirement_template_id" },
                values: new object[,]
                {
                    { 10, "Номер офиса: ", true, 4, 1 },
                    { 11, "Ваша проблема: ", true, 5, 1 }
                });

            migrationBuilder.InsertData(
                schema: "dictionaries",
                table: "requirement_category",
                columns: new[] { "id", "description", "has_agreement", "requirement_category_type_id" },
                values: new object[,]
                {
                    { 1, "Отсутствие доступа к сайту", false, 1 },
                    { 2, "Подключение к онлайн конференции", false, 1 },
                    { 3, "Создание онлайн конференции", false, 1 },
                    { 4, "Компьютер", false, 2 },
                    { 5, "Ноутбук", false, 2 },
                    { 6, "Перемещение рабочего места", false, 2 },
                    { 7, "Принтер (МФУ)", false, 2 },
                    { 8, "Принтер (МФУ) / Замена картриджа", false, 2 },
                    { 9, "Переадресация электронной почты", false, 3 },
                    { 10, "Создание почтового ящика", false, 3 },
                    { 11, "Удаление почтового ящика", false, 3 },
                    { 12, "Пополнение счета", false, 4 },
                    { 13, "Другое", false, 5 },
                    { 14, "Настройка почтового ящика", false, 5 },
                    { 15, "Настройка приложения", false, 5 },
                    { 16, "1С: CRM", false, 6 },
                    { 17, "1С: ECM", false, 6 },
                    { 18, "1С: ERP", false, 6 },
                    { 19, "Завершение сеанса 1C", false, 6 },
                    { 20, "Настройка", false, 6 },
                    { 21, "Удаление", false, 6 },
                    { 22, "Установка", false, 6 },
                    { 23, "Подключение к Wi-Fi", false, 7 },
                    { 24, "Настройка", false, 8 },
                    { 25, "Другое", false, 9 },
                    { 26, "Переадресация", false, 9 },
                    { 27, "Предоставление записи разговора", false, 9 }
                });

            migrationBuilder.InsertData(
                schema: "business",
                table: "subdivision_link_subdivision",
                columns: new[] { "id", "subdivision_id", "subdivision_parent_id" },
                values: new object[,]
                {
                    { 1, 1, null },
                    { 2, 2, 1 }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "user",
                columns: new[] { "id", "email", "name", "object_sid", "password", "role_id" },
                values: new object[,]
                {
                    { 1, "valdemar.leontev@yandex.ru", "Valdemar", null, "WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=", 1 },
                    { 2, "leonetx@yandex.ru", "Bill", null, "WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=", 2 }
                });

            migrationBuilder.InsertData(
                schema: "business",
                table: "file",
                columns: new[] { "id", "creation_date", "hash", "name", "uid", "upload_user_id" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2023, 6, 30, 3, 46, 0, 307, DateTimeKind.Unspecified).AddTicks(8317), new TimeSpan(0, 0, 0, 0, 0)), "", "Инструкция для пользования Helpdesk.pdf", "c7f82c174aae445aa4ebe1d4f3a54ace", 2 });

            migrationBuilder.InsertData(
                schema: "business",
                table: "notification",
                columns: new[] { "id", "creation_date", "is_deleted", "is_read", "message", "recipient_user_id" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2023, 6, 30, 3, 46, 0, 305, DateTimeKind.Unspecified).AddTicks(7807), new TimeSpan(0, 0, 0, 0, 0)), false, false, "Пользователь Vladimir отправил вам на согласование заявку на тему 'ТЗ программистам'", 2 });

            migrationBuilder.InsertData(
                schema: "business",
                table: "profile",
                columns: new[] { "id", "first_name", "last_name", "position_id", "user_id" },
                values: new object[,]
                {
                    { 1, "Vladimir", "Leontev", 3, 1 },
                    { 2, "Bill", "Gates", 2, 2 }
                });

            migrationBuilder.InsertData(
                schema: "business",
                table: "variant",
                columns: new[] { "id", "description", "question_id" },
                values: new object[,]
                {
                    { 40, "1201", 10 },
                    { 41, "1202", 10 },
                    { 42, "1203", 10 },
                    { 43, "1204", 10 },
                    { 44, "1205", 10 },
                    { 45, "1206", 10 },
                    { 46, "1207", 10 },
                    { 47, "1208", 10 },
                    { 48, "1209", 10 },
                    { 49, "1210", 10 },
                    { 50, "1211", 10 },
                    { 51, "1212", 10 },
                    { 52, "1213", 10 },
                    { 53, "1214", 10 },
                    { 54, "1215", 10 },
                    { 55, "1216", 10 },
                    { 56, "1217", 10 }
                });

            migrationBuilder.InsertData(
                schema: "business",
                table: "profile_link_subdivision",
                columns: new[] { "id", "is_head", "profile_id", "subdivision_id" },
                values: new object[] { 1, true, 2, 1 });

            migrationBuilder.InsertData(
                schema: "business",
                table: "requirement",
                columns: new[] { "id", "creation_date", "name", "outgoing_number", "profile_id", "requirement_category_id", "requirement_state_id", "requirement_template_id" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2023, 6, 30, 3, 46, 0, 306, DateTimeKind.Unspecified).AddTicks(2499), new TimeSpan(0, 0, 0, 0, 0)), "ТЗ программистам", 1, 1, 7, 1, 1 });

            migrationBuilder.InsertData(
                schema: "business",
                table: "requirement_category_link_profile",
                columns: new[] { "id", "profile_id", "requirement_category_id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 }
                });

            migrationBuilder.InsertData(
                schema: "business",
                table: "requirement_comment",
                columns: new[] { "id", "description", "requirement_id", "sender_profile_id" },
                values: new object[] { 1, "That's all trash! Let's do it again!", 1, 2 });

            migrationBuilder.InsertData(
                schema: "business",
                table: "requirement_stage",
                columns: new[] { "id", "creation_date", "profile_id", "requirement_id", "state_id" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2023, 6, 30, 3, 46, 0, 305, DateTimeKind.Unspecified).AddTicks(8072), new TimeSpan(0, 0, 0, 0, 0)), 1, 1, 1 });

            migrationBuilder.InsertData(
                schema: "business",
                table: "requirement_stage_link_requirement_comment",
                columns: new[] { "id", "requirement_comment_id", "requirement_stage_id" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "ix_file_uid",
                schema: "business",
                table: "file",
                column: "uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_file_upload_user_id",
                schema: "business",
                table: "file",
                column: "upload_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_recipient_user_id",
                schema: "business",
                table: "notification",
                column: "recipient_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_description",
                schema: "dictionaries",
                table: "position",
                column: "description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_profile_position_id",
                schema: "business",
                table: "profile",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "ix_profile_user_id",
                schema: "business",
                table: "profile",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_profile_link_subdivision_profile_id",
                schema: "business",
                table: "profile_link_subdivision",
                column: "profile_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_profile_link_subdivision_profile_id_subdivision_id",
                schema: "business",
                table: "profile_link_subdivision",
                columns: new[] { "profile_id", "subdivision_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_profile_link_subdivision_subdivision_id",
                schema: "business",
                table: "profile_link_subdivision",
                column: "subdivision_id");

            migrationBuilder.CreateIndex(
                name: "ix_question_question_type_id",
                schema: "business",
                table: "question",
                column: "question_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_question_requirement_template_id",
                schema: "business",
                table: "question",
                column: "requirement_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_profile_id",
                schema: "business",
                table: "requirement",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_requirement_category_id",
                schema: "business",
                table: "requirement",
                column: "requirement_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_requirement_state_id",
                schema: "business",
                table: "requirement",
                column: "requirement_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_requirement_template_id",
                schema: "business",
                table: "requirement",
                column: "requirement_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_category_description_requirement_category_type_",
                schema: "dictionaries",
                table: "requirement_category",
                columns: new[] { "description", "requirement_category_type_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_category_requirement_category_type_id",
                schema: "dictionaries",
                table: "requirement_category",
                column: "requirement_category_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_category_link_profile_profile_id",
                schema: "business",
                table: "requirement_category_link_profile",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_category_link_profile_requirement_category_id_p",
                schema: "business",
                table: "requirement_category_link_profile",
                columns: new[] { "requirement_category_id", "profile_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_category_type_description",
                schema: "dictionaries",
                table: "requirement_category_type",
                column: "description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_comment_requirement_id",
                schema: "business",
                table: "requirement_comment",
                column: "requirement_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_comment_sender_profile_id",
                schema: "business",
                table: "requirement_comment",
                column: "sender_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_link_file_file_id",
                schema: "business",
                table: "requirement_link_file",
                column: "file_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_link_file_requirement_id_file_id",
                schema: "business",
                table: "requirement_link_file",
                columns: new[] { "requirement_id", "file_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_link_notification_notification_id",
                schema: "business",
                table: "requirement_link_notification",
                column: "notification_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_link_notification_requirement_id",
                schema: "business",
                table: "requirement_link_notification",
                column: "requirement_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_link_profile_profile_id",
                schema: "business",
                table: "requirement_link_profile",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_link_profile_requirement_id_profile_id",
                schema: "business",
                table: "requirement_link_profile",
                columns: new[] { "requirement_id", "profile_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_stage_profile_id",
                schema: "business",
                table: "requirement_stage",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_stage_requirement_id",
                schema: "business",
                table: "requirement_stage",
                column: "requirement_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_stage_state_id",
                schema: "business",
                table: "requirement_stage",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirement_stage_link_requirement_comment_requirement_comm",
                schema: "business",
                table: "requirement_stage_link_requirement_comment",
                column: "requirement_comment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_stage_link_requirement_comment_requirement_stag",
                schema: "business",
                table: "requirement_stage_link_requirement_comment",
                column: "requirement_stage_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_requirement_stage_link_requirement_comment_requirement_stag1",
                schema: "business",
                table: "requirement_stage_link_requirement_comment",
                columns: new[] { "requirement_stage_id", "requirement_comment_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subdivision_description",
                schema: "dictionaries",
                table: "subdivision",
                column: "description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subdivision_link_subdivision_subdivision_id",
                schema: "business",
                table: "subdivision_link_subdivision",
                column: "subdivision_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subdivision_link_subdivision_subdivision_id_subdivision_par",
                schema: "business",
                table: "subdivision_link_subdivision",
                columns: new[] { "subdivision_id", "subdivision_parent_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subdivision_link_subdivision_subdivision_parent_id",
                schema: "business",
                table: "subdivision_link_subdivision",
                column: "subdivision_parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_email_object_sid",
                schema: "admin",
                table: "user",
                columns: new[] { "email", "object_sid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_role_id",
                schema: "admin",
                table: "user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_answer_profile_id",
                schema: "business",
                table: "user_answer",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_answer_question_id",
                schema: "business",
                table: "user_answer",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_answer_requirement_id",
                schema: "business",
                table: "user_answer",
                column: "requirement_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_session_user_id",
                schema: "admin",
                table: "user_session",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_variant_question_id",
                schema: "business",
                table: "variant",
                column: "question_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profile_link_subdivision",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement_category_link_profile",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement_link_file",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement_link_notification",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement_link_profile",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement_stage_link_requirement_comment",
                schema: "business");

            migrationBuilder.DropTable(
                name: "subdivision_link_subdivision",
                schema: "business");

            migrationBuilder.DropTable(
                name: "user_answer",
                schema: "business");

            migrationBuilder.DropTable(
                name: "user_session",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "variant",
                schema: "business");

            migrationBuilder.DropTable(
                name: "file",
                schema: "business");

            migrationBuilder.DropTable(
                name: "notification",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement_comment",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement_stage",
                schema: "business");

            migrationBuilder.DropTable(
                name: "subdivision",
                schema: "dictionaries");

            migrationBuilder.DropTable(
                name: "question",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement",
                schema: "business");

            migrationBuilder.DropTable(
                name: "question_type",
                schema: "dictionaries");

            migrationBuilder.DropTable(
                name: "profile",
                schema: "business");

            migrationBuilder.DropTable(
                name: "requirement_category",
                schema: "dictionaries");

            migrationBuilder.DropTable(
                name: "requirement_state",
                schema: "dictionaries");

            migrationBuilder.DropTable(
                name: "requirement_template",
                schema: "business");

            migrationBuilder.DropTable(
                name: "position",
                schema: "dictionaries");

            migrationBuilder.DropTable(
                name: "user",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "requirement_category_type",
                schema: "dictionaries");

            migrationBuilder.DropTable(
                name: "role",
                schema: "dictionaries");
        }
    }
}

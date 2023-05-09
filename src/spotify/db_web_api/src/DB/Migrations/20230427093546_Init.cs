using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DB.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictApplications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClientSecret = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ConsentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    DisplayNames = table.Column<string>(type: "text", nullable: true),
                    Permissions = table.Column<string>(type: "text", nullable: true),
                    PostLogoutRedirectUris = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    RedirectUris = table.Column<string>(type: "text", nullable: true),
                    Requirements = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictScopes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Descriptions = table.Column<string>(type: "text", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    DisplayNames = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    Resources = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "playlist",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    playlist_type = table.Column<int>(type: "integer", nullable: false),
                    genre_type = table.Column<int>(type: "integer", nullable: false),
                    img_src = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    verified = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlist", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "premium",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    users_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_premium", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    country = table.Column<int>(type: "integer", nullable: true),
                    profile_img = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("profile_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_profile",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "song",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    origin_playlist_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    source = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_song", x => x.id);
                    table.ForeignKey(
                        name: "fk_song",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictAuthorizations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ApplicationId = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    Scopes = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictAuthorizations_OpenIddictApplications_Application~",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    playlist_id = table.Column<int>(type: "integer", nullable: true),
                    genre_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_genre",
                        column: x => x.playlist_id,
                        principalTable: "playlist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "liked_playlist",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    playlist_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("liked_playlist_pkey", x => new { x.user_id, x.playlist_id });
                    table.ForeignKey(
                        name: "fk_liked_playlist_playlist_id",
                        column: x => x.playlist_id,
                        principalTable: "playlist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_liked_playlist_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_premium",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    premium_id = table.Column<int>(type: "integer", nullable: false),
                    start_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("premium_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_premium",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_premium_premium_premium_id",
                        column: x => x.premium_id,
                        principalTable: "premium",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlist_song",
                columns: table => new
                {
                    playlist_id = table.Column<int>(type: "integer", nullable: false),
                    song_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("playlist_song_pkey", x => new { x.playlist_id, x.song_id });
                    table.ForeignKey(
                        name: "fk_playlist_song_playlist_id",
                        column: x => x.playlist_id,
                        principalTable: "playlist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_playlist_song_song_id",
                        column: x => x.song_id,
                        principalTable: "song",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ApplicationId = table.Column<string>(type: "text", nullable: true),
                    AuthorizationId = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Payload = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    RedemptionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferenceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "OpenIddictAuthorizations",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9396abb9-0688-4dd2-b478-fea77b497486", "0089d4be-3e46-47f8-bf50-9c0759756353", "User", "USER" },
                    { "afe7d127-5169-47b1-91b3-5b0cdc1268bc", "99d22cf7-8012-4845-ba7c-848a0f6ad5c5", "Admin", "ADMIN" },
                    { "e3864a44-1314-4095-bd0e-672fc569b9e2", "50e1e34d-e208-44ac-b245-0d76b23e05a3", "Artist", "ARTIST" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "id", "AccessFailedCount", "ConcurrencyStamp", "email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "120877ed-84b9-4ed5-9b87-d78965fc4fe0", 0, "37285e0f-b3c2-4a75-85f6-73a3c4c6da29", "user02@gamil.com", true, false, null, "USER02@GMAIL.COM", "USER02@GMAIL.COM", "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==", null, false, "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4", false, "user02@gmail.com" },
                    { "26ecd8e6-2f2c-4385-b855-aa0a2bd8d429", 0, "37285e0f-b3c2-4a75-85f6-73a3c4c6da29", "admin01@gmail.com", true, false, null, "ADMIN01@GMAIL.COM", "ADMIN01@GMAIL.COM", "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==", null, false, "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4", false, "admin01@gmail.com" },
                    { "4b89cecf-5188-44bf-94b2-6eb0cdf8da02", 0, "37285e0f-b3c2-4a75-85f6-73a3c4c6da29", "admin03@gmail.comm", true, false, null, "ADMIN03@GMAIL.COM", "ADMIN03@GMAIL.COM", "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==", null, false, "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4", false, "admin03@gmail.com" },
                    { "5f34130c-2ed9-4c83-a600-e474e8f43bac", 0, "37285e0f-b3c2-4a75-85f6-73a3c4c6da29", "user03@gmail.com", true, false, null, "USER03@GMAIL.COM", "USER03@GMAIL.COM", "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==", null, false, "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4", false, "user03@gmail.com" },
                    { "5f34130c-2ed9-4c83-a600-e474e8f44bac", 0, "37285e0f-b3c2-4a75-85f6-73a3c4c6da29", "user04@gmail.com", true, false, null, "USER04@GMAIL.COM", "USER04@GMAIL.COM", "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==", null, false, "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4", false, "user04@gmail.com" },
                    { "5f34130c-2ed9-4c83-a600-e474e8f48bac", 0, "37285e0f-b3c2-4a75-85f6-73a3c4c6da29", "user01@gmail.com", true, false, null, "USER01@GMAIL.COM", "USER01@GMAIL.COM", "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==", null, false, "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4", false, "user01@gmail.com" },
                    { "ca2aa01b-a215-4611-838a-f11b9552103e", 0, "37285e0f-b3c2-4a75-85f6-73a3c4c6da29", "admin02@gamil.com", true, false, null, "ADMIN02@GMAIL.COM", "ADMIN02@GMAIL.COM", "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==", null, false, "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4", false, "admin02@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "playlist",
                columns: new[] { "id", "genre_type", "img_src", "playlist_type", "title", "user_id", "verified" },
                values: new object[,]
                {
                    { 1, 4, "playlist_test.jpg", 4, "LikedSongs", "5f34130c-2ed9-4c83-a600-e474e8f48bac", true },
                    { 2, 3, "playlist_test.jpg", 3, "simple playlist", "5f34130c-2ed9-4c83-a600-e474e8f48bac", true }
                });

            migrationBuilder.InsertData(
                table: "premium",
                columns: new[] { "id", "description", "name", "price", "users_count" },
                values: new object[,]
                {
                    { 1, "Some Description", "Individual", 10.0, 1 },
                    { 2, "Some Description", "Student", 7.5, 1 },
                    { 3, "Some Description", "Duo", 12.0, 2 },
                    { 4, "Some Description", "Family", 16.0, 2 }
                });

            migrationBuilder.InsertData(
                table: "profile",
                columns: new[] { "user_id", "birthday", "country", "profile_img", "user_type", "username" },
                values: new object[,]
                {
                    { "120877ed-84b9-4ed5-9b87-d78965fc4fe0", new DateOnly(1996, 2, 23), 3, "src2", 0, "user02" },
                    { "26ecd8e6-2f2c-4385-b855-aa0a2bd8d429", new DateOnly(1985, 2, 21), 0, "src5", 2, "admin01" },
                    { "4b89cecf-5188-44bf-94b2-6eb0cdf8da02", new DateOnly(1995, 2, 21), 2, "src7", 2, "admin03" },
                    { "5f34130c-2ed9-4c83-a600-e474e8f43bac", new DateOnly(2008, 3, 16), 0, "src3", 0, "user03" },
                    { "5f34130c-2ed9-4c83-a600-e474e8f44bac", new DateOnly(1987, 2, 21), 2, "src4", 0, "user04" },
                    { "5f34130c-2ed9-4c83-a600-e474e8f48bac", new DateOnly(2000, 8, 3), 0, "src1", 0, "user01" },
                    { "ca2aa01b-a215-4611-838a-f11b9552103e", new DateOnly(1998, 2, 21), 2, "src6", 2, "admin02" }
                });

            migrationBuilder.InsertData(
                table: "song",
                columns: new[] { "id", "name", "origin_playlist_id", "source", "user_id" },
                values: new object[,]
                {
                    { 1, "song1", 0, "pp.mp3", "5f34130c-2ed9-4c83-a600-e474e8f48bac" },
                    { 2, "song2", 0, "hp.mp3", "5f34130c-2ed9-4c83-a600-e474e8f48bac" },
                    { 3, "song3", 0, "src3", "5f34130c-2ed9-4c83-a600-e474e8f48bac" },
                    { 4, "song4", 0, "src4", "5f34130c-2ed9-4c83-a600-e474e8f48bac" },
                    { 5, "song5", 0, "src5", "5f34130c-2ed9-4c83-a600-e474e8f48bac" }
                });

            migrationBuilder.InsertData(
                table: "user_premium",
                columns: new[] { "user_id", "end_at", "premium_id", "start_at" },
                values: new object[,]
                {
                    { "120877ed-84b9-4ed5-9b87-d78965fc4fe0", new DateTime(2020, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "5f34130c-2ed9-4c83-a600-e474e8f48bac", new DateTime(2020, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_genre_playlist_id",
                table: "genre",
                column: "playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_liked_playlist_playlist_id",
                table: "liked_playlist",
                column: "playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_ClientId",
                table: "OpenIddictApplications",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type",
                table: "OpenIddictAuthorizations",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictScopes_Name",
                table: "OpenIddictScopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type",
                table: "OpenIddictTokens",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ReferenceId",
                table: "OpenIddictTokens",
                column: "ReferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_playlist_song_song_id",
                table: "playlist_song",
                column: "song_id");

            migrationBuilder.CreateIndex(
                name: "IX_song_user_id",
                table: "song",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_premium_premium_id",
                table: "user_premium",
                column: "premium_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "liked_playlist");

            migrationBuilder.DropTable(
                name: "OpenIddictScopes");

            migrationBuilder.DropTable(
                name: "OpenIddictTokens");

            migrationBuilder.DropTable(
                name: "playlist_song");

            migrationBuilder.DropTable(
                name: "profile");

            migrationBuilder.DropTable(
                name: "user_premium");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OpenIddictAuthorizations");

            migrationBuilder.DropTable(
                name: "playlist");

            migrationBuilder.DropTable(
                name: "song");

            migrationBuilder.DropTable(
                name: "premium");

            migrationBuilder.DropTable(
                name: "OpenIddictApplications");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

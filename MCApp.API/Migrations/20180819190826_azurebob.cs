using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MCApp.API.Migrations
{
    public partial class azurebob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    KnownAs = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastActive = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Username", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Accountname = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastInterestMutation = table.Column<DateTime>(nullable: false),
                    InterestPeriod = table.Column<string>(nullable: true),
                    LastActive = table.Column<DateTime>(nullable: false),
                    Percentage = table.Column<double>(nullable: false),
                    CalculatedInterest = table.Column<double>(nullable: false),
                    Balance = table.Column<double>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    LastMutationCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    InterestDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Percentage = table.Column<double>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interests_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mutations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrevId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    InterestDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Balance = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mutations", x => x.Id);
                    table.UniqueConstraint("AK_Mutations_PrevId", x => x.PrevId);
                    table.ForeignKey(
                        name: "FK_Mutations_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mutations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Interests_AccountId",
                table: "Interests",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Interests_UserId_AccountId",
                table: "Interests",
                columns: new[] { "UserId", "AccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_Mutations_AccountId",
                table: "Mutations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Mutations_Created",
                table: "Mutations",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Mutations_UserId_AccountId",
                table: "Mutations",
                columns: new[] { "UserId", "AccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "Mutations");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

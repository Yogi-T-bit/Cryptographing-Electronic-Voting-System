using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptographicElectronicVotingSystem.Dal.Migrations
{
    /// <inheritdoc />
    public partial class UserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            /*migrationBuilder.CreateTable(
                name: "candidates",
                columns: table => new
                {
                    CandidateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Party = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidates", x => x.CandidateID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tallyingcenters",
                columns: table => new
                {
                    CenterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CenterPublicKey = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tallyingcenters", x => x.CenterID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
                

            migrationBuilder.CreateTable(
                name: "votetally",
                columns: table => new
                {
                    TallyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CenterID = table.Column<int>(type: "int", nullable: true),
                    CandidateID = table.Column<int>(type: "int", nullable: true),
                    VoteCount = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_votetally", x => x.TallyID);
                    table.ForeignKey(
                        name: "FK_votetally_candidates_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "candidates",
                        principalColumn: "CandidateID");
                    table.ForeignKey(
                        name: "FK_votetally_tallyingcenters_CenterID",
                        column: x => x.CenterID,
                        principalTable: "tallyingcenters",
                        principalColumn: "CenterID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");*/

            /*migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VoterId = table.Column<long>(type: "bigint", nullable: false),
                    VoterID = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");*/

            /*migrationBuilder.CreateTable(
                name: "voters",
                columns: table => new
                {
                    VoterID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAuthorized = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasVoted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VoterPublicKey = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplicationUserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voters", x => x.VoterID);
                    table.ForeignKey(
                        name: "FK_voters_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");*/

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    VoteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VoterID = table.Column<long>(type: "bigint", nullable: true),
                    CandidateID = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "current_timestamp()"),
                    VoteProof = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_votes", x => x.VoteID);
                    table.ForeignKey(
                        name: "FK_votes_candidates_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "candidates",
                        principalColumn: "CandidateID");
                    table.ForeignKey(
                        name: "FK_votes_voters_VoterID",
                        column: x => x.VoterID,
                        principalTable: "voters",
                        principalColumn: "VoterID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            /*migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VoterID",
                table: "AspNetUsers",
                column: "VoterID");*/

            /*migrationBuilder.CreateIndex(
                name: "IX_voters_ApplicationUserId",
                table: "voters",
                column: "ApplicationUserId",
                unique: true);*/
            
            Console.WriteLine("IX_votes_CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_votes_CandidateID",
                table: "votes",
                column: "CandidateID");
            
            Console.WriteLine("IX_votes_VoterID");

            migrationBuilder.CreateIndex(
                name: "IX_votes_VoterID",
                table: "votes",
                column: "VoterID");
            
            Console.WriteLine("IX_votetally_CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_votetally_CandidateID",
                table: "votetally",
                column: "CandidateID");
            
            Console.WriteLine("IX_votetally_CenterID");

            migrationBuilder.CreateIndex(
                name: "IX_votetally_CenterID",
                table: "votetally",
                column: "CenterID");

            Console.WriteLine("FK_AspNetUsers_voters_VoterID");
            
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_voters_VoterID",
                table: "AspNetUsers",
                column: "VoterID",
                principalTable: "voters",
                principalColumn: "VoterID",
                onDelete: ReferentialAction.Cascade);
            
            Console.WriteLine("FK_voters_AspNetUsers_ApplicationUserId");
            
            // add foreign key to ApplicationUser
            migrationBuilder.AddForeignKey(
                name: "FK_voters_AspNetUsers_ApplicationUserId",
                table: "voters",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        // <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_voters_VoterID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "votes");

            migrationBuilder.DropTable(
                name: "votetally");

            migrationBuilder.DropTable(
                name: "candidates");

            migrationBuilder.DropTable(
                name: "tallyingcenters");

            migrationBuilder.DropTable(
                name: "voters");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

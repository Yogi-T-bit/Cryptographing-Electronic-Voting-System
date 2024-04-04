using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptographingElectronicVotingSystem.Dal.Migrations
{
    /// <inheritdoc />
    public partial class VoterRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "VoterId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "candidates",
                columns: table => new
                {
                    CandidateID = table.Column<int>(type: "int", nullable: false),
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
                name: "voters",
                columns: table => new
                {
                    VoterID = table.Column<long>(type: "bigint", nullable: false),
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
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "votetally",
                columns: table => new
                {
                    TallyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CenterID = table.Column<int>(type: "int", nullable: true),
                    tallyingcenterCenterID = table.Column<int>(type: "int", nullable: false),
                    CandidateID = table.Column<int>(type: "int", nullable: true),
                    VoteCount = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_votetally_tallyingcenters_tallyingcenterCenterID",
                        column: x => x.tallyingcenterCenterID,
                        principalTable: "tallyingcenters",
                        principalColumn: "CenterID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    VoteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VoterID = table.Column<long>(type: "bigint", nullable: true),
                    CandidateID = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VoterId",
                table: "AspNetUsers",
                column: "VoterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voters_ApplicationUserId",
                table: "voters",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_votes_CandidateID",
                table: "votes",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_votes_VoterID",
                table: "votes",
                column: "VoterID");

            migrationBuilder.CreateIndex(
                name: "IX_votetally_CandidateID",
                table: "votetally",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_votetally_tallyingcenterCenterID",
                table: "votetally",
                column: "tallyingcenterCenterID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_voters_VoterId",
                table: "AspNetUsers",
                column: "VoterId",
                principalTable: "voters",
                principalColumn: "VoterID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_voters_VoterId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "votes");

            migrationBuilder.DropTable(
                name: "votetally");

            migrationBuilder.DropTable(
                name: "voters");

            migrationBuilder.DropTable(
                name: "candidates");

            migrationBuilder.DropTable(
                name: "tallyingcenters");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_VoterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "AspNetUsers");
        }
    }
}

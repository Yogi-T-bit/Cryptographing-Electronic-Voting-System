using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptographicElectronicVotingSystem.Dal.Data.Migrations
{
    /// <inheritdoc />
    public partial class VoterRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "votes");

            migrationBuilder.DropTable(
                name: "votetally");

            migrationBuilder.DropTable(
                name: "candidates");

            migrationBuilder.DropTable(
                name: "tallyingcenters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
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
                    CenterPublicKey = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tallyingcenters", x => x.CenterID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    VoteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CandidateID = table.Column<int>(type: "int", nullable: true),
                    VoterID = table.Column<long>(type: "bigint", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "votetally",
                columns: table => new
                {
                    TallyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CandidateID = table.Column<int>(type: "int", nullable: true),
                    TallyingcenterCenterID = table.Column<int>(type: "int", nullable: false),
                    CenterID = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_votetally_tallyingcenters_TallyingcenterCenterID",
                        column: x => x.TallyingcenterCenterID,
                        principalTable: "tallyingcenters",
                        principalColumn: "CenterID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "IX_votetally_TallyingcenterCenterID",
                table: "votetally",
                column: "TallyingcenterCenterID");
        }
    }
}

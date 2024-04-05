﻿// <auto-generated />
using System;
using CryptographicElectronicVotingSystem.Dal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CryptographicElectronicVotingSystem.Dal.Migrations
{
    [DbContext(typeof(ElectronicVotingSystemContext))]
    [Migration("20240405063752_VoterRelationship")]
    partial class VoterRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<long>("VoterID")
                        .HasColumnType("bigint");

                    b.Property<long?>("VoterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VoterID");

                    b.ToTable("AspNetUsers", t =>
                        {
                            t.HasTrigger("AspNetUsers_Trigger");
                        });
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate", b =>
                {
                    b.Property<int>("CandidateID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Party")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("CandidateID");

                    b.ToTable("candidates", t =>
                        {
                            t.HasTrigger("candidates_Trigger");
                        });
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter", b =>
                {
                    b.Property<int>("CenterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CenterPublicKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("CenterID");

                    b.ToTable("tallyingcenters", t =>
                        {
                            t.HasTrigger("tallyingcenters_Trigger");
                        });
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote", b =>
                {
                    b.Property<int>("VoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CandidateID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<string>("VoteProof")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("VoterID")
                        .HasColumnType("bigint");

                    b.HasKey("VoteID");

                    b.HasIndex("CandidateID");

                    b.HasIndex("VoterID");

                    b.ToTable("votes", t =>
                        {
                            t.HasTrigger("votes_Trigger");
                        });
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", b =>
                {
                    b.Property<long>("VoterID")
                        .HasColumnType("bigint");

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("HasVoted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsAuthorized")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("VoterPublicKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("VoterID");

                    b.HasIndex("ApplicationUserId")
                        .IsUnique();

                    b.ToTable("voters", t =>
                        {
                            t.HasTrigger("voters_Trigger");
                        });
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally", b =>
                {
                    b.Property<int>("TallyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CandidateID")
                        .HasColumnType("int");

                    b.Property<int?>("CenterID")
                        .HasColumnType("int");

                    b.Property<int?>("VoteCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("'0'");

                    b.HasKey("TallyID");

                    b.HasIndex("CandidateID");

                    b.HasIndex("CenterID");

                    b.ToTable("votetally", t =>
                        {
                            t.HasTrigger("votetally_Trigger");
                        });
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", b =>
                {
                    b.HasOne("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", "Voter")
                        .WithMany()
                        .HasForeignKey("VoterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Voter");
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote", b =>
                {
                    b.HasOne("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate", "candidate")
                        .WithMany("votes")
                        .HasForeignKey("CandidateID");

                    b.HasOne("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", "voter")
                        .WithMany("votes")
                        .HasForeignKey("VoterID");

                    b.Navigation("candidate");

                    b.Navigation("voter");
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", b =>
                {
                    b.HasOne("CryptographicElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", "ApplicationUser")
                        .WithOne()
                        .HasForeignKey("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", "ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally", b =>
                {
                    b.HasOne("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate", "candidate")
                        .WithMany("votetallies")
                        .HasForeignKey("CandidateID");

                    b.HasOne("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter", "tallyingcenter")
                        .WithMany("votetallies")
                        .HasForeignKey("CenterID");

                    b.Navigation("candidate");

                    b.Navigation("tallyingcenter");
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate", b =>
                {
                    b.Navigation("votes");

                    b.Navigation("votetallies");
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter", b =>
                {
                    b.Navigation("votetallies");
                });

            modelBuilder.Entity("CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", b =>
                {
                    b.Navigation("votes");
                });
#pragma warning restore 612, 618
        }
    }
}

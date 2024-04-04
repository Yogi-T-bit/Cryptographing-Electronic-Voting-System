﻿// <auto-generated />
using System;
using CryptographingElectronicVotingSystem.Dal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CryptographingElectronicVotingSystem.Dal.Migrations
{
    [DbContext(typeof(ApplicationIdentityDbContext))]
    partial class ApplicationIdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<int?>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.HasIndex("TenantId");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationTenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Hosts")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AspNetTenants");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<int?>("TenantId")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<long?>("VoterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("TenantId");

                    b.HasIndex("VoterId")
                        .IsUnique();

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate", b =>
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

                    b.ToTable("candidates");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter", b =>
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

                    b.ToTable("tallyingcenters");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote", b =>
                {
                    b.Property<int>("VoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CandidateID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("VoteProof")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("VoterID")
                        .HasColumnType("bigint");

                    b.HasKey("VoteID");

                    b.HasIndex("CandidateID");

                    b.HasIndex("VoterID");

                    b.ToTable("votes");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", b =>
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

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("voters");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally", b =>
                {
                    b.Property<int>("TallyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CandidateID")
                        .HasColumnType("int");

                    b.Property<int?>("CenterID")
                        .HasColumnType("int");

                    b.Property<int?>("VoteCount")
                        .HasColumnType("int");

                    b.Property<int>("tallyingcenterCenterID")
                        .HasColumnType("int");

                    b.HasKey("TallyID");

                    b.HasIndex("CandidateID");

                    b.HasIndex("tallyingcenterCenterID");

                    b.ToTable("votetally");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationRole", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationTenant", "ApplicationTenant")
                        .WithMany("Roles")
                        .HasForeignKey("TenantId");

                    b.Navigation("ApplicationTenant");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationTenant", "ApplicationTenant")
                        .WithMany("Users")
                        .HasForeignKey("TenantId");

                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", "Voter")
                        .WithOne()
                        .HasForeignKey("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", "VoterId");

                    b.Navigation("ApplicationTenant");

                    b.Navigation("Voter");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate", "candidate")
                        .WithMany("votes")
                        .HasForeignKey("CandidateID");

                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", "voter")
                        .WithMany("votes")
                        .HasForeignKey("VoterID");

                    b.Navigation("candidate");

                    b.Navigation("voter");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate", "candidate")
                        .WithMany("votetallies")
                        .HasForeignKey("CandidateID");

                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter", "tallyingcenter")
                        .WithMany("votetallies")
                        .HasForeignKey("tallyingcenterCenterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("candidate");

                    b.Navigation("tallyingcenter");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.Authentication.ApplicationTenant", b =>
                {
                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate", b =>
                {
                    b.Navigation("votes");

                    b.Navigation("votetallies");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter", b =>
                {
                    b.Navigation("votetallies");
                });

            modelBuilder.Entity("CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter", b =>
                {
                    b.Navigation("votes");
                });
#pragma warning restore 612, 618
        }
    }
}

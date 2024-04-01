using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;
using CryptographingElectronicVotingSystem.Dal.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CryptographingElectronicVotingSystem.Dal.Data
{
    public partial class ElectronicVotingSystemContext : DbContext
    {
        public ElectronicVotingSystemContext()
        {
        }

        public ElectronicVotingSystemContext(DbContextOptions<ElectronicVotingSystemContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote>()
              .HasOne(i => i.candidate)
              .WithMany(i => i.votes)
              .HasForeignKey(i => i.CandidateID)
              .HasPrincipalKey(i => i.CandidateID);

            builder.Entity<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote>()
              .HasOne(i => i.voter)
              .WithMany(i => i.votes)
              .HasForeignKey(i => i.VoterID)
              .HasPrincipalKey(i => i.VoterID);

            builder.Entity<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally>()
              .HasOne(i => i.candidate)
              .WithMany(i => i.votetallies)
              .HasForeignKey(i => i.CandidateID)
              .HasPrincipalKey(i => i.CandidateID);

            builder.Entity<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally>()
              .HasOne(i => i.tallyingcenter)
              .WithMany(i => i.votetallies)
              .HasForeignKey(i => i.CenterID)
              .HasPrincipalKey(i => i.CenterID);

            builder.Entity<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote>()
              .Property(p => p.Timestamp)
              .HasDefaultValueSql(@"current_timestamp()");     

            builder.Entity<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally>()
              .Property(p => p.VoteCount)
              .HasDefaultValueSql(@"'0'");

            builder.Entity<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote>()
              .Property(p => p.Timestamp)
              .HasColumnType("datetime");
            
            this.OnModelBuilding(builder);
            
        }

        public DbSet<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> candidates { get; set; }

        public DbSet<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> tallyingcenters { get; set; }

        public DbSet<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> voters { get; set; }

        public DbSet<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote> votes { get; set; }

        public DbSet<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> votetallies { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}
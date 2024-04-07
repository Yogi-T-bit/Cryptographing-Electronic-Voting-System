using System;
using System.Linq;
using System.Collections.Generic;
using CryptographicElectronicVotingSystem.Models.CryptographicElectronicVotingSystem;
using Microsoft.EntityFrameworkCore;

namespace CryptographicElectronicVotingSystem.Data
{
    public partial class CryptographicElectronicVotingSystemContext : DbContext
    {
        public CryptographicElectronicVotingSystemContext()
        {
        }

        public CryptographicElectronicVotingSystemContext(DbContextOptions<CryptographicElectronicVotingSystemContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Vote>()
              .HasOne(i => i.Voter)
              .WithMany(i => i.Votes)
              .HasForeignKey(i => i.VoterID)
              .HasPrincipalKey(i => i.VoterID);

            builder.Entity<Votetally>()
              .HasOne(i => i.Candidate)
              .WithMany(i => i.Votetallies)
              .HasForeignKey(i => i.CandidateID)
              .HasPrincipalKey(i => i.CandidateID);

            builder.Entity<Votetally>()
              .HasOne(i => i.Tallyingcenter)
              .WithMany(i => i.Votetallies)
              .HasForeignKey(i => i.CenterID)
              .HasPrincipalKey(i => i.CenterID);

            builder.Entity<Vote>()
              .Property(p => p.Timestamp)
              .HasDefaultValueSql(@"current_timestamp()");     

            builder.Entity<Votetally>()
              .Property(p => p.VoteCount)
              .HasDefaultValueSql(@"'0'");     

            builder.Entity<Vote>()
              .Property(p => p.Timestamp)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<Tallyingcenter> Tallyingcenters { get; set; }

        public DbSet<Voter> Voters { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Votetally> Votetallies { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}
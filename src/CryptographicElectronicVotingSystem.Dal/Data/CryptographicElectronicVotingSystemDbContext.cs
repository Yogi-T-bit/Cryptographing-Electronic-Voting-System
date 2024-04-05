using System;
using System.Linq;
using System.Collections.Generic;
using CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity;
using Microsoft.EntityFrameworkCore;
using CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem;
using Microsoft.AspNetCore.Identity;

namespace CryptographicElectronicVotingSystem.Dal.Data
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
            
            builder.Ignore<ApplicationUser>();
            builder.Ignore<ApplicationRole>();
            builder.Ignore<IdentityUser>();
            builder.Ignore<IdentityRole>();
            builder.Ignore<IdentityUserRole<string>>();
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();
            
            // Voter as a foreign key to ApplicationUser
            builder.Entity<Voter>()
              .HasOne(v => v.ApplicationUser)
              .WithOne() // Assuming there is no navigation property back to Voter in ApplicationUser
              .HasForeignKey<Voter>(v => v.ApplicationUserId);

            builder.Entity<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote>()
              .HasOne(i => i.Candidate)
              .WithMany(i => i.Votes)
              .HasForeignKey(i => i.CandidateID)
              .HasPrincipalKey(i => i.CandidateID);

            builder.Entity<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote>()
              .HasOne(i => i.Voter)
              .WithMany(i => i.Votes)
              .HasForeignKey(i => i.VoterID)
              .HasPrincipalKey(i => i.VoterID);

            builder.Entity<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally>()
              .HasOne(i => i.Candidate)
              .WithMany(i => i.Votetallies)
              .HasForeignKey(i => i.CandidateID)
              .HasPrincipalKey(i => i.CandidateID);

            builder.Entity<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally>()
              .HasOne(i => i.Tallyingcenter)
              .WithMany(i => i.Votetallies)
              .HasForeignKey(i => i.CenterID)
              .HasPrincipalKey(i => i.CenterID);

            builder.Entity<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote>()
              .Property(p => p.Timestamp)
              .HasDefaultValueSql(@"current_timestamp()");     

            builder.Entity<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally>()
              .Property(p => p.VoteCount)
              .HasDefaultValueSql(@"'0'");     

            builder.Entity<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote>()
              .Property(p => p.Timestamp)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> Candidates { get; set; }

        public DbSet<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> Tallyingcenters { get; set; }

        public DbSet<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> Voters { get; set; }

        public DbSet<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> Votes { get; set; }

        public DbSet<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> Votetallies { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}
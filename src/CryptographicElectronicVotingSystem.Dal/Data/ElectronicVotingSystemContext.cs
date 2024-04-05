using System;
using System.Linq;
using System.Collections.Generic;
using CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;
using Microsoft.EntityFrameworkCore;
using CryptographicElectronicVotingSystem.Dal.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CryptographicElectronicVotingSystem.Dal.Data
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
            builder.Ignore<ApplicationUser>();
            builder.Ignore<ApplicationRole>();
            builder.Ignore<IdentityUser>();
            builder.Ignore<IdentityRole>();
            builder.Ignore<IdentityUserRole<string>>();
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();
            
            // voter as a foreign key to ApplicationUser
            builder.Entity<voter>()
              .HasOne(v => v.ApplicationUser)
              .WithOne() // Assuming there is no navigation property back to Voter in ApplicationUser
              .HasForeignKey<voter>(v => v.ApplicationUserId);

            builder.Entity<vote>()
              .HasOne(i => i.candidate)
              .WithMany(i => i.votes)
              .HasForeignKey(i => i.CandidateID)
              .HasPrincipalKey(i => i.CandidateID);

            builder.Entity<vote>()
              .HasOne(i => i.voter)
              .WithMany(i => i.votes)
              .HasForeignKey(i => i.VoterID)
              .HasPrincipalKey(i => i.VoterID);

            builder.Entity<votetally>()
              .HasOne(i => i.candidate)
              .WithMany(i => i.votetallies)
              .HasForeignKey(i => i.CandidateID)
              .HasPrincipalKey(i => i.CandidateID);

            builder.Entity<votetally>()
              .HasOne(i => i.tallyingcenter)
              .WithMany(i => i.votetallies)
              .HasForeignKey(i => i.CenterID)
              .HasPrincipalKey(i => i.CenterID);

            builder.Entity<vote>()
              .Property(p => p.Timestamp)
              .HasDefaultValueSql(@"current_timestamp()");     

            builder.Entity<votetally>()
              .Property(p => p.VoteCount)
              .HasDefaultValueSql(@"'0'");

            builder.Entity<vote>()
              .Property(p => p.Timestamp)
              .HasColumnType("datetime");
            
            this.OnModelBuilding(builder);
            
        }

        public DbSet<candidate> candidates { get; set; }

        public DbSet<tallyingcenter> tallyingcenters { get; set; }

        public DbSet<voter> voters { get; set; }

        public DbSet<vote> votes { get; set; }

        public DbSet<votetally> votetallies { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}
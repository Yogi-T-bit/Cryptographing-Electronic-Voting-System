using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using CryptographicElectronicVotingSystem.Dal.Models;
using CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity;
using CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem;

namespace CryptographicElectronicVotingSystem.Dal.Data
{
    public partial class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        {
        }

        public ApplicationIdentityDbContext()
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // exclude the following tables from the identity model (vote, Votetally, candidate, Tallyingcenter)
            builder.Ignore<Vote>();
            builder.Ignore<Votetally>();
            builder.Ignore<Candidate>();
            builder.Ignore<Tallyingcenter>();
            
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Voter) // ApplicationUser has one Voter
                .WithOne() // Assuming no navigation property back from Voter to ApplicationUser
                .HasForeignKey<ApplicationUser>(u => u.VoterId); // ApplicationUser uses VoterId as foreign key

            builder.Entity<ApplicationUser>()
                   .HasMany(u => u.Roles)
                   .WithMany(r => r.Users)
                   .UsingEntity<IdentityUserRole<string>>();

            this.OnModelBuilding(builder);
        }
    }
}
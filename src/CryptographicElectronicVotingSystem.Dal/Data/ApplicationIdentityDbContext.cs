using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Dal.Models.Authentication;
using CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

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
            builder.Ignore<voter>();
            builder.Ignore<vote>();
            builder.Ignore<votetally>();
            builder.Ignore<candidate>();
            builder.Ignore<tallyingcenter>();
            
            // Configure the one-to-one relationship between ApplicationUser and Voter
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Voter) // ApplicationUser has one Voter
                .WithOne() // Assuming no navigation property back from Voter to ApplicationUser
                .HasForeignKey<ApplicationUser>(u => u.VoterId); // Applic

            builder.Entity<ApplicationUser>()
               .HasMany(u => u.Roles)
               .WithMany(r => r.Users)
               .UsingEntity<IdentityUserRole<string>>(
                userRole => userRole.HasOne<ApplicationRole>().
                       WithMany()
                       .HasForeignKey(ur => ur.RoleId)
                       .IsRequired(),
                userRole => userRole.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired());
            
            this.OnModelBuilding(builder);
        }
    }
}
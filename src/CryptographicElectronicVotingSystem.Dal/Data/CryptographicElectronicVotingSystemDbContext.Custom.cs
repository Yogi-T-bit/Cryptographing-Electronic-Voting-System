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
        partial void OnModelBuilding(ModelBuilder builder)
        {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        }
    }
}
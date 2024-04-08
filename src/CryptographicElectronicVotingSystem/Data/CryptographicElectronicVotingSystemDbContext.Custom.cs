using CryptographicElectronicVotingSystem.Models.ApplicationIdentity;
using Microsoft.EntityFrameworkCore;

namespace CryptographicElectronicVotingSystem.Data
{
    public partial class CryptographicElectronicVotingSystemContext : DbContext
    {
        partial void OnModelBuilding(ModelBuilder builder)
        {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        }
    }
}
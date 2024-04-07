using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace CryptographicElectronicVotingSystem.Models.ApplicationIdentity
{
    public partial class ApplicationRole : IdentityRole
    {
        [JsonIgnore]
        public ICollection<ApplicationUser> Users { get; set; }

    }
}
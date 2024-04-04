using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;
using Microsoft.AspNetCore.Identity;

namespace CryptographicElectronicVotingSystem.Dal.Models.Authentication
{
    [Table("AspNetUsers")]
    public partial class ApplicationUser : IdentityUser
    {
        [JsonIgnore, IgnoreDataMember]
        public override string PasswordHash { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        [JsonIgnore, IgnoreDataMember, NotMapped]
        public string Name
        {
            get
            {
                return UserName;
            }
            set
            {
                UserName = value;
            }
        }

        public ICollection<ApplicationRole> Roles { get; set; }
        
        // Add a foreign key for Voter
        public long? VoterId { get; set; }

        // Navigation property to Voter
        public virtual voter Voter { get; set; }
    }
}
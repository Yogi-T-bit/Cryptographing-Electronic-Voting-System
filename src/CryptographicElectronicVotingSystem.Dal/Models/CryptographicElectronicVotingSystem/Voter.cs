using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity;

namespace CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem
{
    [Table("voters")]
    public partial class Voter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VoterID { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsAuthorized { get; set; }

        public bool HasVoted { get; set; }

        public string VoterPublicKey { get; set; }

        public ICollection<Vote> Votes { get; set; }
        
        // Add a foreign key property for ApplicationUser
        public string ApplicationUserId { get; set; }

        // Navigation property to ApplicationUser
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
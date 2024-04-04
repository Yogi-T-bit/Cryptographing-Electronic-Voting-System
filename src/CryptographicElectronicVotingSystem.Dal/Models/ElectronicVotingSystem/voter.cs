using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptographicElectronicVotingSystem.Dal.Models.Authentication;

namespace CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem
{
    [Table("voters")]
    public partial class voter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Changed from Identity to None
        public long VoterID { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsAuthorized { get; set; }

        public bool HasVoted { get; set; }

        public string VoterPublicKey { get; set; }

        public ICollection<vote> votes { get; set; }
        
        // Add a foreign key property for ApplicationUser
        public string ApplicationUserId { get; set; }

        // Navigation property to ApplicationUser
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
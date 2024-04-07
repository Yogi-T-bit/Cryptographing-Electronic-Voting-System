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
        [Required]
        public long VoterID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Email { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string LastName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string FirstName { get; set; }

        [ConcurrencyCheck]
        public bool IsAuthorized { get; set; }

        [ConcurrencyCheck]
        public bool? HasVoted { get; set; }

        [ConcurrencyCheck]
        public string? VoterPrivateKey { get; set; }

        [ConcurrencyCheck]
        public string UserId { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
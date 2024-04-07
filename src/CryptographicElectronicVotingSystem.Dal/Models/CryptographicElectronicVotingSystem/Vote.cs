using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem
{
    [Table("votes")]
    public partial class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteID { get; set; }

        [ConcurrencyCheck]
        public long? VoterID { get; set; }

        public Voter Voter { get; set; }

        [ConcurrencyCheck]
        public DateTime? Timestamp { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string VotePublicKey { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string EncryptedVote { get; set; }

    }
}
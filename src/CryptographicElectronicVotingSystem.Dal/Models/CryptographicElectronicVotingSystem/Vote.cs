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

        public long? VoterID { get; set; }

        public Voter Voter { get; set; }

        public int? CandidateID { get; set; }

        public Candidate Candidate { get; set; }

        public DateTime? Timestamp { get; set; }

        [Required]
        public string VoteProof { get; set; }

    }
}
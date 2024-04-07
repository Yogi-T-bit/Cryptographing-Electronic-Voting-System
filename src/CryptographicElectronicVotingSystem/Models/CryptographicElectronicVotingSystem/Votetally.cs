using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptographicElectronicVotingSystem.Models.CryptographicElectronicVotingSystem;

namespace CryptographicElectronicVotingSystem.Models.CryptographicElectronicVotingSystem
{
    [Table("votetally")]
    public partial class Votetally
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TallyID { get; set; }

        [ConcurrencyCheck]
        public int? CenterID { get; set; }

        public Tallyingcenter Tallyingcenter { get; set; }

        [ConcurrencyCheck]
        public int? CandidateID { get; set; }

        public Candidate Candidate { get; set; }

        [ConcurrencyCheck]
        public int? VoteCount { get; set; }

    }
}
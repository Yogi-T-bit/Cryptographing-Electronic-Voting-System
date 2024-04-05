using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem
{
    [Table("votetally")]
    public partial class Votetally
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TallyID { get; set; }

        public int? CenterID { get; set; }

        public Tallyingcenter Tallyingcenter { get; set; }

        public int? CandidateID { get; set; }

        public Candidate Candidate { get; set; }

        public int? VoteCount { get; set; }

    }
}
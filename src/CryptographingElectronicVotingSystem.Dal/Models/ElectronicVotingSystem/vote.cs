using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem
{
    [Table("votes")]
    public partial class vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteID { get; set; }

        public int? VoterID { get; set; }

        public voter voter { get; set; }

        public int? CandidateID { get; set; }

        public candidate candidate { get; set; }

        public DateTime? Timestamp { get; set; }

        public string VoteProof { get; set; }

    }
}
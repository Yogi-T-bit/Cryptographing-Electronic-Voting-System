using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem
{
    [Table("votetally")]
    public partial class votetally
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TallyID { get; set; }

        public int? CenterID { get; set; }

        public tallyingcenter tallyingcenter { get; set; }

        public int? CandidateID { get; set; }

        public candidate candidate { get; set; }

        public int? VoteCount { get; set; }

    }
}
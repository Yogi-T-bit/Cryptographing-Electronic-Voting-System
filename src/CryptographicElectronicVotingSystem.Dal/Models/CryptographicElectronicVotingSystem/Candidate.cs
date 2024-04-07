using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem
{
    [Table("candidates")]
    public partial class Candidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CandidateID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Name { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Party { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<Votetally> Votetallies { get; set; }

    }
}
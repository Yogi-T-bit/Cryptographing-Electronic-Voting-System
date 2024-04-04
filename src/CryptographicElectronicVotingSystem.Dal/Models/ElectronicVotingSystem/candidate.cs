using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem
{
    [Table("candidates")]
    public partial class candidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Changed from Identity to None
        public int CandidateID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Party { get; set; }

        public ICollection<vote> votes { get; set; }

        public ICollection<votetally> votetallies { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem
{
    [Table("voters")]
    public partial class voter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoterID { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsAuthorized { get; set; }

        public bool HasVoted { get; set; }

        public string VoterPublicKey { get; set; }

        public ICollection<vote> votes { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem
{
    [Table("tallyingcenters")]
    public partial class Tallyingcenter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CenterID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Name { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Location { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string CenterPublicKey { get; set; }

        public ICollection<Votetally> Votetallies { get; set; }

    }
}
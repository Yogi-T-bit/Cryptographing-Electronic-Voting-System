using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem
{
    [Table("tallyingcenters")]
    public partial class tallyingcenter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CenterID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }

        public string CenterPublicKey { get; set; }

        public ICollection<votetally> votetallies { get; set; }

    }
}
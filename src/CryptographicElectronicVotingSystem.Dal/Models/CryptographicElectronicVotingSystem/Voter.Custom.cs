using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity;

namespace CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem
{
    //[Table("voters")]
    public partial class Voter
    {
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
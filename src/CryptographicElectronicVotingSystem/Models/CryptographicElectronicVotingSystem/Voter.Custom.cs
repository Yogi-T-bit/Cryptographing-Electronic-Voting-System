using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptographicElectronicVotingSystem.Models.ApplicationIdentity;

namespace CryptographicElectronicVotingSystem.Models.CryptographicElectronicVotingSystem
{
    //[Table("voters")]
    public partial class Voter
    {
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
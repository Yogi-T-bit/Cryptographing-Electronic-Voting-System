using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem;
using Microsoft.AspNetCore.Identity;

namespace CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity
{
    //[Table("AspNetUsers")]
    public partial class ApplicationUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }
}
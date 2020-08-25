using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CarMD.Shell.Api.Models
{
    [DataContract]
    public class ReportEmail
    {
        [DataMember]
        [Required]
        public Guid ReportId { get; set; }

        [DataMember]
        [Required]
        public string Email { get; set; }

    }
}
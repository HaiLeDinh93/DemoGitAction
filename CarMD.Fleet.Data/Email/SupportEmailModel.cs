using CarMD.Fleet.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace CarMD.Fleet.Data.Email
{
    public class SupportEmailModel : EmailTemplate
    {
        [Required]
        public string Topic { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserEmail { get; set; }

        public string UserPhone { get; set; }

        public string Extenstion { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsSendForCustomer { get; set; }

        public int RefNumber { get; set; }

        public string EmailTemplateImgPath { get; set; }
    }
}

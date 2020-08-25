using System;

namespace CarMD.Fleet.Data.Email
{
    public class ResetPasswordEmailModel : EmailTemplate
    {
        public string UserEmail { get; set; }

        public string ResetLogintUrl { get; set; }

        public string EmailTemplateImgPath { get; set; }
    }
}

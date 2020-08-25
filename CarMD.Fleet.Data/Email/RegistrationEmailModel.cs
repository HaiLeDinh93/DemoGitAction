using System;


namespace CarMD.Fleet.Data.Email
{
    public class RegistrationEmailModel : EmailTemplate
    {
        public string UserEmail { get; set; }

        public string ResetLogintUrl { get; set; }

        public string EmailTemplateImgPath { get; set; }
    }
}

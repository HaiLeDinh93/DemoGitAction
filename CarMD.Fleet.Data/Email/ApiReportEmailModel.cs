

namespace CarMD.Fleet.Data.Email
{
    public class ApiReportEmailModel : EmailTemplate
    {
        public string Logo { set; get; }

        public string ImageLine { get; set; }

        public string HealthReportUrl { get; set; }

        public string ShellReportUrl { get; set; }
    }
}

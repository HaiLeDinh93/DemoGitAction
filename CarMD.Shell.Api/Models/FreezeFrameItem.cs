namespace CarMD.Shell.Api.Models
{
    public class FreezeFrameItem
    {
        public FreezeFrameItem(string description, string value)
        {
            Description = description;
            Value = value;
        }

        public string Description { get; set; }

        public string Value { get; set; }
    }
}
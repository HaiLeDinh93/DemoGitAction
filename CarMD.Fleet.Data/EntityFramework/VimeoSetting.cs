using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class VimeoSetting : BaseEntity
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public string VideoId { get; set; }
        public int Duration { get; set; }
        public string PictureId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Type { get; set; }
    }
}

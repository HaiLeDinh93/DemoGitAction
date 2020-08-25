using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto.Metafuse;
using CarMD.Fleet.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class TSBMapper : BaseMapper<TSBInfo, TSB>
    {
        public override void Map(TSBInfo info, TSB target)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            target.AutoSystem = info.AutoSystem;
            target.CreatedDateTime = DateTime.Parse(info.CreatedDateString, culture);
            target.Description = info.Description;
            target.FileNamePDF = info.FileNamePDF;
            target.IssueDate = DateTime.Parse(info.IssueDateString, culture);
            target.ManufacturerNumber = info.ManufacturerNumber;
            target.TSBID = info.TsbId;
            target.PDFFileUrl = info.PDFFileUrl;
            target.TSBText = info.TsbText;
            target.UpdatedDateTime = DateTime.Parse(info.UpdatedDateString, culture);
            //map DTC code
            target.DTCs = new List<string>();
            if (info.DTCcodes != null && info.DTCcodes.Length > 0)
            {
                foreach (var s in info.DTCcodes)
                {
                    target.DTCs.Add(s);
                }
            }

            //map tsb category
            target.TsbCategories = new List<int>();
            if (info.TSBCategories != null && info.TSBCategories.Count() > 0)
            {
                foreach (var category in info.TSBCategories)
                {
                    target.TsbCategories.Add(category.Id);
                }
            }
            target.TsbTypes = new List<string>();
            if (info.TSBTypes != null && info.TSBTypes.Count() > 0)
            {
                foreach (var type in info.TSBTypes)
                {
                    target.TsbTypes.Add(type.Description);
                }
            }
        }
    }
}

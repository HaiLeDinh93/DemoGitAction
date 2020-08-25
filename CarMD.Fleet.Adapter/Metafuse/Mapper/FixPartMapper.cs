using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class FixPartMapper : BaseMapper<FixPartInfo, FixPart>
    {
        public override void Map(FixPartInfo source, FixPart target)
        {
            target.Description = source.Description;
            target.ManufacturerName = source.ManufacturerName;
            target.Name = source.Name;
            target.PartNumber = source.PartNumber;
            target.Price = source.Price;
            target.Quantity = source.Quantity;

            if (source.FixPartOemInfos == null || !source.FixPartOemInfos.Any()) return;

            target.FixPartOEMs = new List<FixPartOEM>();
            foreach (var fixPartOem in source.FixPartOemInfos)
            {
                target.FixPartOEMs.Add(MetafuseMapper.MapFixPartOEM(fixPartOem));
            }
        }
    }
}

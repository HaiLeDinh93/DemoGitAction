using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class DLCLocationMapper : BaseMapper<DLCLocationInfo, DLCLocation>
    {
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public override void Map(DLCLocationInfo source, DLCLocation target)
        {
            target.Make = source.Make;
            target.Year = source.Year;
            target.Model = source.Model;
            target.Access = source.Access;
            target.Comments = source.Comments;
            target.LocationNumber = source.LocationNumber;
            target.ImageFileName = source.ImageFileName;
            target.ImageFileUrl = source.ImageFileUrl;
            target.ImageFileUrlSmall = source.ImageFileUrlSmall;
        }
    }
}

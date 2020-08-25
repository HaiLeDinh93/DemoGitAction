namespace CarMD.Fleet.Core.Mapper
{
    public interface IMapper<S, T>
    {
        /// <summary>
        /// map source data to new target entity
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        T Map(S source);

        /// <summary>
        /// map source data to existed entity
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void Map(S source, T target);
    }
}
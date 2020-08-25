using System;

namespace CarMD.Fleet.Core.Mapper
{
    public abstract class BaseMapper<S, T> : IMapper<S, T>
    {
        public virtual T Map(S source)
        {
            var result = Activator.CreateInstance<T>();
            Map(source, result);
            return result;
        }

        public abstract void Map(S source, T target);
    }
}
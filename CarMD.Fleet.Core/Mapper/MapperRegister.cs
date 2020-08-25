using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMD.Fleet.Core.Mapper
{
    public sealed class MapperRegister
    {
        private readonly IDictionary<Type, IDictionary<Type, object>> _mappers =
            new Dictionary<Type, IDictionary<Type, object>>();

        public void Register<TS, T>(IMapper<TS, T> mapper)
        {
            var sType = typeof (TS);
            var tType = typeof (T);
            if (_mappers.ContainsKey(sType))
            {
                var sub = _mappers[sType];
                if (!sub.ContainsKey(tType))
                {
                    sub[tType] = mapper;
                }
            }
            else
            {
                _mappers[sType] = new Dictionary<Type, object>();
                _mappers[sType].Add(tType, mapper);
            }
        }

        public IMapper<TS, T> Get<TS, T>()
        {
            var sType = typeof (TS);
            var tType = typeof (T);
            if (_mappers.ContainsKey(sType))
            {
                var sub = _mappers[sType];
                if (sub.ContainsKey(tType))
                {
                    return sub[tType] as IMapper<TS, T>;
                }
            }
            return null;
        }

        public T Map<TS, T>(TS source)
        {
            var mapper = Get<TS, T>();
            if (mapper == null)
            {
                throw new Exception(string.Format("Cannot find the mapper: Source {0}, Target: {1}", typeof (TS),
                                                  typeof (T)));
            }
            return mapper.Map(source);
        }

        public void Map<TS, T>(TS source, T target)
        {
            var mapper = Get<TS, T>();
            if (mapper == null)
            {
                throw new Exception(string.Format("Cannot find the mapper: Source {0}, Target: {1}", typeof (TS),
                                                  typeof (T)));
            }
            mapper.Map(source, target);
        }

        public IList<T> MapList<TS, T>(IList<TS> sources) where T : class
        {
            var mapper = Get<TS, T>();
            if (mapper == null)
            {
                throw new Exception(string.Format("Cannot find the mapper: Source {0}, Target: {1}", typeof (TS),
                                                  typeof (T)));
            }

            return sources.Select(item => mapper.Map(item)).ToList();
        }

        public IList<T> MapArray<TS, T>(TS[] sources) where T : class
        {
            var mapper = Get<TS, T>();
            if (mapper == null)
            {
                throw new Exception(string.Format("Cannot find the mapper: Source {0}, Target: {1}", typeof (TS),
                                                  typeof (T)));
            }

            return sources.Select(item => mapper.Map(item)).ToList();
        }
    }
}
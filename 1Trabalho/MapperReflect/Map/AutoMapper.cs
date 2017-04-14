using System;
using System.Collections.Generic;

namespace MapperReflect
{
    public class AutoMapper {
        private static Dictionary<KeyValuePair<Type, Type>, IMapper> cacheContainer = Cache.cache;
        public static IMapper Build(Type klassSrc, Type klassDest) {
            IMapper cache;
            KeyValuePair<Type, Type> typePair = new KeyValuePair<Type, Type>(klassSrc, klassDest);
            cacheContainer.TryGetValue(typePair, out cache);
            if (cache == null){
                cache = new Mapper(klassSrc, klassDest);
                cacheContainer.Add(typePair, cache);
            }
            return cache;
        }
    }
}
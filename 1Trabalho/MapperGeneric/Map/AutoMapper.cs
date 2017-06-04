using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperGeneric
{
    public class AutoMapper {
        private static Dictionary<KeyValuePair<Type, Type>, IMapper> cacheContainer = Cache.cache;

        /* Get the Mapper of parameters types if it's already in cache otherwise creates and add it to cache. */
        public static IMapper Build(Type klassSrc, Type klassDest)
        {
            if (((klassSrc.IsValueType && !IsStructType(klassSrc)) || klassSrc == typeof(string)) ||
               ((klassDest.IsValueType && !IsStructType(klassDest)) || klassDest == typeof(string))) return null;

            IMapper cache;
            KeyValuePair<Type, Type> typePair = new KeyValuePair<Type, Type>(klassSrc, klassDest);
            cacheContainer.TryGetValue(typePair, out cache);
            if (cache == null)
            {
               
                cache = new Mapper(klassSrc, klassDest);
                cacheContainer.Add(typePair, cache);
            }
            return cache;
        }
        
        public static IMapperGeneric<TSrc, TDest> Build <TSrc, TDest> ()
        {
            Type TSource = typeof(TSrc), TDestiny = typeof(TDest);

            if (((TSource.IsValueType && !IsStructType(TSource)) || TSource == typeof(string)) ||
               ((TDestiny.IsValueType && !IsStructType(TDestiny)) || TDestiny == typeof(string))) return null;

            IMapper cache;
            KeyValuePair<Type, Type> typePair = new KeyValuePair<Type, Type>(TSource, TDestiny);
            cacheContainer.TryGetValue(typePair, out cache);
            if (cache == null)
            {
                cache = new Mapper<TSrc, TDest>(TSource, TDestiny);
                cacheContainer.Add(typePair, cache);
            }
            return (IMapperGeneric<TSrc, TDest>) cache;
        }


        /* Verify if the type received in parameters is a struct. */
        public static bool IsStructType(Type type)
        {
            return type.IsValueType && !type.IsPrimitive;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperEmit
{
    class AutoMapperEmitter {
        private static Dictionary<KeyValuePair<Type, Type>, IMapper> cacheContainer = Cache.cache;
        private static ModuleBuilder moduleBuilder;

        /* Get the Mapper of parameters types if it's already in cache otherwise creates and add it to cache. */
        public static IMapper Build(Type klassSrc, Type klassDest)
        {
            if (((klassSrc.IsValueType && !IsStructType(klassSrc)) || klassSrc == typeof(string)) ||
               ((klassDest.IsValueType && !IsStructType(klassDest)) || klassDest == typeof(string))) return null;

            if (moduleBuilder == null) {
                AssemblyName aName = new AssemblyName("MappingAssembly");
                AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
                moduleBuilder = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
            }

            IMapper cache;
            KeyValuePair<Type, Type> typePair = new KeyValuePair<Type, Type>(klassSrc, klassDest);
            cacheContainer.TryGetValue(typePair, out cache);
            if (cache == null)
            {
                cache = new Mapper(klassSrc, klassDest, moduleBuilder);
                cacheContainer.Add(typePair, cache);
            }
            return cache;
        }

        /* Verify if the type received in parameters is a struct. */
        private static bool IsStructType(Type type)
        {
            return !type.IsPrimitive && type.Namespace != null && !type.Namespace.StartsWith("System.");
        }
    }
}

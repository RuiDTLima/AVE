
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingProperties : Mapping
    {

        public override object Map(object srcObject, Type src, Type dest, Dictionary<String, String> dict){
            PropertyInfo[] srcProperties;
            PropertyInfo destiny, origin;
            String currentName;
            Type currentDestType, currentSrcType;

            /* Get destiny type object. */

            object destObject = init(srcObject, src, dest, out srcProperties);

            /* For each source property map it's corresponding property in destination. */

            for (int i = 0; i < srcProperties.Length; i++) {
                /* Get current destination property. */

                origin = srcProperties[i];
                dict.TryGetValue(origin.Name, out currentName);
                destiny = currentName == null ? dest.GetProperty(origin.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance): 
                                                 dest.GetProperty(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (destiny == null) continue;

                currentDestType = destiny.GetType();
                currentSrcType = origin.GetType();

                /* If the type is equal between source and destination current properties then sets values
                 * else checks if their types are not primitive types and are compatible and map them. */

                if (currentDestType.Equals(currentSrcType))
                    destiny.SetValue(destObject, origin.GetValue(srcObject));
                else if (!currentSrcType.IsValueType && !currentDestType.IsValueType && !currentDestType.IsSubclassOf(currentSrcType)) {
                    IMapper cache;
                    KeyValuePair<Type, Type> typePair = new KeyValuePair<Type, Type>(currentSrcType, currentDestType);
                    cacheContainer.TryGetValue(typePair, out cache);

                    if (cache == null){
                        cache = AutoMapper.Build(currentSrcType, currentDestType);
                        cacheContainer.Add(typePair, cache);
                    }

                    cache.Map(origin);
                }
            }
            return destObject;
        }

        public object init(object srcObject, Type src, Type dest, out PropertyInfo[] srcProperties) {
            srcProperties = null;
            if (!srcObject.GetType().Equals(src))
                return null;

            srcProperties = srcObject.GetType().GetProperties();
            if (dest.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(dest);
            return getAvailableConstructor(srcProperties, dest).Invoke(new Object[srcProperties.Length]);
        }

        private ConstructorInfo getAvailableConstructor(PropertyInfo[] srcProperties, Type dest)
        {
            int size = srcProperties.Length;
            Type[] propertiesTypes = new Type[size];
            for (int i = 0; i < size; ++i) propertiesTypes[i] = srcProperties[i].PropertyType;
            return dest.GetConstructor(propertiesTypes);
        }

    }
}

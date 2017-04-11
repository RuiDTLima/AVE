using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingProperties : Mapping
    {
        public MappingProperties() { }

        public MappingProperties(Type source, Type destino) : base(source, destino)
        {

        }

        //TODO finish
        public override object Map(object src, Dictionary<String, String> dict)
        {
            if (!src.GetType().Equals(this.src))
                return null;
            PropertyInfo[] srcProperties = src.GetType().GetProperties();
            object instance = Activator.CreateInstance(dest);
            PropertyInfo[] destProperties = dest.GetProperties();
            PropertyInfo destiny, origin;
            String propertyName;
            Type destType, srcType;
            Dictionary<KeyValuePair<Type, Type>, IMapper> cacheContainer = Cache.cache;
            for (int i = 0; i < srcProperties.Length; i++)
            {
                origin = srcProperties[i];
                dict.TryGetValue(origin.Name, out propertyName);
                destiny = propertyName == null ? dest.GetProperty(origin.Name) : destiny = dest.GetProperty(propertyName);
                destType = destiny.GetType();
                srcType = origin.GetType();
                if (destType.Equals(srcType))
                    destiny.SetValue(instance, origin.GetValue(src));
                else if (!srcType.IsValueType && !destType.IsValueType && !destType.IsSubclassOf(srcType))
                {
                    IMapper cache;
                    KeyValuePair<Type, Type> typePair = new KeyValuePair<Type, Type>(srcType, destType);
                    cacheContainer.TryGetValue(typePair, out cache);
                    if (cache == null)
                    {
                        cache = AutoMapper.Build(srcType, destType);
                        cacheContainer.Add(typePair, cache);
                    }
                    cache.Map(origin);
                }
            }
            return instance;
        }

    }
}

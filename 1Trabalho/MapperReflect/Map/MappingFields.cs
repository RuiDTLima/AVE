using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingFields : Mapping
    {
        public override object Map(object srcObject, Type src, Type dest, Dictionary<String, String> dict){
            FieldInfo[] srcFields;
            FieldInfo destiny, origin;
            String currentName;
            Type currentDestType, currentSrcType;

            /* Get destiny type object. */

            object destObject = init(srcObject, src, dest, out srcFields);

            /* For each source field map it's corresponding field in destination. */

            for (int i = 0; i < srcFields.Length; i++){
                
                /* Get current destination field. */

                origin = srcFields[i];
                dict.TryGetValue(origin.Name, out currentName);
                destiny = currentName == null ? dest.GetField(origin.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) :
                                                 dest.GetField(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (destiny == null) continue;

                currentDestType = destiny.GetType();
                currentSrcType = origin.GetType();

                /* If the type is equal between source and destination current fields then sets values
                 * else checks if their types are not primitive types and are compatible and map them. */

                if (currentDestType.Equals(currentSrcType))
                    destiny.SetValue(destObject, origin.GetValue(srcObject));
                else if (!currentSrcType.IsValueType && !currentDestType.IsValueType && !currentDestType.IsSubclassOf(currentSrcType)){
                    IMapper cache;
                    KeyValuePair<Type, Type> typePair = new KeyValuePair<Type, Type>(currentSrcType, currentDestType);
                    cacheContainer.TryGetValue(typePair, out cache);

                    if (cache == null) {
                        cache = AutoMapper.Build(currentSrcType, currentDestType);
                        cacheContainer.Add(typePair, cache);
                    }

                    cache.Map(origin);
                }
            }
            return destObject;
        }

        public object init(object srcObject, Type src, Type dest, out FieldInfo[] srcFields) {
            srcFields = null;
            if (!srcObject.GetType().Equals(src))
                return null;

            srcFields = srcObject.GetType().GetFields();
            if (dest.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(dest);
            return getAvailableConstructor(srcFields, dest).Invoke(new Object[srcFields.Length]);
        }

        private ConstructorInfo getAvailableConstructor(FieldInfo[] srcFields, Type dest) {
            int size = srcFields.Length;
            Type[] propertiesTypes = new Type[size];
            for (int i = 0; i < size; ++i) propertiesTypes[i] = srcFields[i].FieldType;
            return dest.GetConstructor(propertiesTypes);
        }

    }
}

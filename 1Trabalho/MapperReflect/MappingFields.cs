using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingFields : Mapping
    {
        public MappingFields(){}

        public MappingFields(Type source, Type destino) : base(source, destino){}

        public override object Map(object srcObject, Dictionary<String, String> dict){
            FieldInfo[] srcFields;
            FieldInfo destiny, origin;

            /* Get destiny type object. */

            destObject = init(srcObject, out srcFields);

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

        public object init(object src, out FieldInfo[] srcFields){
            srcFields = null;
            if (!src.GetType().Equals(this.src))
                return null;

            srcFields = src.GetType().GetFields();
            if (dest.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(dest);
            return getAvailableConstructor(srcFields).Invoke(new Object[srcFields.Length]);
        }

        private ConstructorInfo getAvailableConstructor(FieldInfo[] srcFields){
            int size = srcFields.Length;
            Type[] propertiesTypes = new Type[size];
            for (int i = 0; i < size; ++i) propertiesTypes[i] = srcFields[i].FieldType;
            return dest.GetConstructor(propertiesTypes);
        }

    }
}

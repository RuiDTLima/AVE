
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingProperties : Mapping
    {
        public override void Map(object srcObject, object destObject, Dictionary<String, String> dict){
            Map(srcObject, destObject, null, dict);
        }
        public override void Map(object srcObject, object destObject, Type attr, Dictionary<String, String> dict){
            Type src = srcObject.GetType(), dest = destObject.GetType();
            PropertyInfo[] srcProperties = srcObject.GetType().GetProperties();
            PropertyInfo destiny, origin;
            string currentName;
            Type currentDestType, currentSrcType;

            /* For each source property map it's corresponding property in destination. */
            for (int i = 0; i < srcProperties.Length; i++) {

                /* Get current destination property. */
                origin = srcProperties[i];
                object value = origin.GetValue(srcObject);

                if (attr != null && !origin.IsDefined(attr) || value == null) continue;

                dict.TryGetValue(origin.Name, out currentName);
                destiny = currentName == null ? dest.GetProperty(origin.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance): 
                                                 dest.GetProperty(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (destiny == null || (attr != null && !destiny.IsDefined(attr))) continue;
              
                currentDestType = destiny.PropertyType;
                currentSrcType = origin.PropertyType;

                /* If the type is equal between source and destination current properties then sets values
                 * else checks if their types are not primitive types and are compatible and map them. */
                if (currentDestType.Equals(currentSrcType))
                    destiny.SetValue(destObject, value);
                else if (!currentSrcType.IsValueType && !currentDestType.IsValueType && currentSrcType != typeof(string) && currentDestType != typeof(string) && !currentDestType.IsSubclassOf(currentSrcType)) {
                    IMapper aux = AutoMapper.Build(currentSrcType, currentDestType);
                    destiny.SetValue(destObject, joinData(aux.Map(value), aux.Bind(Mapping.Fields).Map(value)));
                }
            }
        }
    }
}

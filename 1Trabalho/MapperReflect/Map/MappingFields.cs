using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingFields : Mapping
    {
        public override void Map(object srcObject, object destObject, Dictionary<String, String> dict) {
            Map(srcObject, destObject, null, dict);
        }
        public override void Map(object srcObject, object destObject, Type attr, Dictionary<String, String> dict){
            Type src = srcObject.GetType(), dest = destObject.GetType();
            FieldInfo[] srcFields = srcObject.GetType().GetFields();
            FieldInfo destiny, origin;
            String currentName;
            Type currentDestType, currentSrcType;
            
            /* For each source field map it's corresponding field in destination. */
            for (int i = 0; i < srcFields.Length; i++){
                
                /* Get current destination field. */
                origin = srcFields[i];
                object value = origin.GetValue(srcObject);
                if (attr != null && !origin.IsDefined(attr) || value == null) continue;
                

                dict.TryGetValue(origin.Name, out currentName);
                destiny = currentName == null ? dest.GetField(origin.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) :
                                                 dest.GetField(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (destiny == null || (attr != null && !destiny.IsDefined(attr))) continue;

                currentDestType = destiny.FieldType;
                currentSrcType = origin.FieldType;

                /* If the type is equal between source and destination current fields then sets values
                 * else checks if their types are not primitive types and are compatible and map them. */
                if (currentDestType == currentSrcType)
                    destiny.SetValue(destObject, value);
                else if (!currentSrcType.IsValueType && !currentDestType.IsValueType && currentSrcType != typeof(string) && currentDestType != typeof(string) && !currentDestType.IsSubclassOf(currentSrcType)){
                    IMapper aux = AutoMapper.Build(currentSrcType, currentDestType);
                    destiny.SetValue(destObject, joinData(aux.Map(value), aux.Bind(Mapping.Fields).Map(value)));
                }
            }
        }
    }
}

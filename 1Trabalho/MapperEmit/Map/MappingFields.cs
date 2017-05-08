using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperEmit {
    public class MappingFields : Mapping {
        /* Auxiliar method to be called without using attribute. */
        public override void Map(object srcObject, object destObject, Dictionary<String, String> dict) {
            Map(srcObject, destObject, null, dict);
        }

        /* For each field in srcObject maps its value in the corresponding field in destObject depending on attribute. */
        public override void Map(object srcObject, object destObject, Type attr, Dictionary<String, String> dict) {
            Type src = srcObject.GetType(), dest = destObject.GetType();
            FieldInfo[] srcFields = srcObject.GetType().GetFields();
            FieldInfo destiny, origin;
            string currentName;
            Type currentDestType, currentSrcType;
            
            /* For each source field map its corresponding field in destination. */
            for (int i = 0; i < srcFields.Length; i++) {
                
                /* Get current origin field. */
                origin = srcFields[i];
                object value = origin.GetValue(srcObject);
                if (attr != null && !origin.IsDefined(attr) || value == null) continue;
                
                /* Get the corresponding destiny field. */
                dict.TryGetValue(origin.Name, out currentName);
                destiny = currentName == null ? dest.GetField(origin.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) :
                                                 dest.GetField(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
       
                if (destiny == null || (attr != null && !destiny.IsDefined(attr))) continue;

                currentDestType = destiny.FieldType;
                currentSrcType = origin.FieldType;

                /* If source and destination type are equal then set destination with source value
                 * else asks for a new mapper and tries to set its value */
                if (currentDestType == currentSrcType)
                    destiny.SetValue(destObject, value);
                else {
                    IMapper aux = AutoMapper.Build(currentSrcType, currentDestType);
                    if(aux != null)
                        destiny.SetValue(destObject, joinData(aux.Bind(Mapping.Properties).Map(value), aux.Bind(Mapping.Fields).Map(value)));
                }
            }
        }
    }
}

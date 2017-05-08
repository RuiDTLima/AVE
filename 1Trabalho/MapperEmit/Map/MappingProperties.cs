using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperEmit {
    public class MappingProperties : Mapping {
        /* Auxiliar method to be called without using attribute. */
        public override void Map(object srcObject, object destObject, Dictionary<String, String> dict) {
            Map(srcObject, destObject, null, dict);
        }
        /* For each property in srcObject maps its value in the corresponding property in destObject depending on attribute. */
        public override void Map(object srcObject, object destObject, Type attr, Dictionary<String, String> dict) {
            Type src = srcObject.GetType(), dest = destObject.GetType();
            PropertyInfo[] srcProperties = srcObject.GetType().GetProperties();
            PropertyInfo destiny, origin;
            string currentName;
            Type currentDestType, currentSrcType;

            /* For each source property map it's corresponding property in destination. */
            for (int i = 0; i < srcProperties.Length; i++) {

                /* Get current origin property. */
                origin = srcProperties[i];
                object value = origin.GetValue(srcObject);

                if (attr != null && !origin.IsDefined(attr) || value == null) continue;

                /* Gets the corresponding destiny property. */
                dict.TryGetValue(origin.Name, out currentName);
                destiny = currentName == null ? dest.GetProperty(origin.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance): 
                                                 dest.GetProperty(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (destiny == null || (attr != null && !destiny.IsDefined(attr))) continue;
              
                currentDestType = destiny.PropertyType;
                currentSrcType = origin.PropertyType;

                /* If source and destination type are equal then set destination with source value
                 * else asks for a new mapper and tries to set its value */
                if (currentDestType.Equals(currentSrcType))
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

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperGeneric {
    public abstract class Emitter {
        public abstract IMappingEmit EmitClass(Type srcType, Type destType, Type attr, Dictionary<string, string> dict, Dictionary<string, Func<object>> dictResult);

        public abstract IConstructorEmit EmitClass(Type destType);


        /* Receive two objects, one only has properties with values and the other only has fields with values
         * combines the information of each object into one and returns it. 
         */
        public static object joinData(object objProperties, object objFields) {
            if ((objProperties == null || objProperties == null) || objProperties.GetType() != objFields.GetType()) return null;

            PropertyInfo[] properties = objFields.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++) {
                PropertyInfo aux = objProperties.GetType().GetProperty(properties[i].Name);
                properties[i].SetValue(objFields, aux.GetValue(objProperties));
            }
            return objFields;
        }
    }
}
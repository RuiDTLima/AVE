using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class Mapping {

        public static MappingFields Fields = new MappingFields();
        public static MappingProperties Properties = new MappingProperties();
        private Type Attribute { get; set; }
        
        public Mapping(Type Attribute) {
            this.Attribute = Attribute;
        }

        public Mapping(){}
        
        /* By default maps properties, but if Attribute isn't null maps fields as well. */
        public virtual void Map(object srcObject, object destObject, Dictionary<String, String> dict) {
            if (Attribute != null)
                Fields.Map(srcObject, destObject, Attribute, dict);
            Properties.Map(srcObject, destObject, Attribute, dict);
        }

        /* To be redefined by extedend classes. */
        public virtual void Map(object srcObject, object destObject, Type Attr, Dictionary<String, String> dict) { }

        /* Receive two objects, one only has properties with values and the other only has fields with values
         * combines the information of each object into one and returns it. */
        protected object joinData(object objProperties, object objFields) {
            if (objProperties.GetType() != objFields.GetType()) return null;

            PropertyInfo [] properties = objFields.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++) {
                PropertyInfo aux = objProperties.GetType().GetProperty(properties[i].Name);
                properties[i].SetValue(objFields, aux.GetValue(objProperties));
            }
            return objFields;
        }

    }
}

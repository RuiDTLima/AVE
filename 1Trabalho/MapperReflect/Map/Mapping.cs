using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class Mapping
    {
        public static MappingFields Fields = new MappingFields();
        public static MappingProperties Properties = new MappingProperties();
        private Type Attribute { get; set; }
        
        public Mapping(Type Attribute) {
            this.Attribute = Attribute;
        }

        public Mapping(){}
        

        public virtual void Map(object srcObject, object destObject, Dictionary<String, String> dict) {
            if (Attribute != null)
                Fields.Map(srcObject, destObject, Attribute, dict);
            Properties.Map(srcObject, destObject, Attribute, dict);
        }
        /* To be redefined by extedend classes. */
        public virtual void Map(object srcObject, object destObject, Type Attr, Dictionary<String, String> dict) { }
        protected object joinData(object objProperties, object objFields){
            PropertyInfo [] properties = objFields.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++) {
                PropertyInfo aux = objProperties.GetType().GetProperty(properties[i].Name);
                properties[i].SetValue(objFields, aux.GetValue(objProperties));
            }
            return objFields;
        }

    }
}

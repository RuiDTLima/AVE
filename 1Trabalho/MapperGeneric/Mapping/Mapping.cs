using System;
using System.Collections.Generic;

namespace MapperGeneric
{
    public class Mapping {
        public static MappingFields Fields { get; set; } = new MappingFields();
        public static MappingProperties Properties { get; set; } = new MappingProperties();
        private Type Attribute { get; set; }

        public Mapping() { }

        public Mapping(Type Attribute) : this() {
            this.Attribute = Attribute;
        }
        
        /* By default maps properties, but if Attribute isn't null maps fields as well. */
        public virtual void Map(object srcObject, object destObject, Dictionary<string, string> dict, Dictionary<string, Func<object>> dictResult) {
            if (Attribute != null)
                Fields.Map(srcObject, destObject, Attribute, dict, dictResult);
            Properties.Map(srcObject, destObject, Attribute, dict, dictResult);
        }
        
        /* To be redefined by extedend classes. */
        public virtual void Map(object srcObject, object destObject, Type Attr, Dictionary<string, string> dict, Dictionary<string, Func<object>> dictResult) {
        }
    }
}
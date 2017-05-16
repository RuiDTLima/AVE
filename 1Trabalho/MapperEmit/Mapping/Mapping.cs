using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperEmit
{
    public class Mapping {

        public static MappingFields Fields { get; set; }
        public static MappingProperties Properties { get; set; }
        private Type Attribute { get; set; }

        public Mapping()
        {
            Fields = new MappingFields();
            Properties = new MappingProperties();
        }

        public Mapping(Type Attribute) : this() {
            this.Attribute = Attribute;
        }
        
        /* By default maps properties, but if Attribute isn't null maps fields as well. */
        public virtual void Map(object srcObject, object destObject, ModuleBuilder mb, Dictionary<String, String> dict) {
            if (Attribute != null)
                Fields.Map(srcObject, destObject, Attribute, mb, dict);
            Properties.Map(srcObject, destObject, Attribute, mb, dict);
        }

        /* To be redefined by extedend classes. */
        public virtual void Map(object srcObject, object destObject, Type Attr, ModuleBuilder mb, Dictionary<String, String> dict) { }
    }
}

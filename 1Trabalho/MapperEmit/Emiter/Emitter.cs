using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MapperEmit.Emiter
{
    public abstract class Emitter {
        public static FieldEmitter Fields = new FieldEmitter();
        public static PropertiesEmitter Properties = new PropertiesEmitter();
        private Type Attribute { get; set; }

        public Emitter() { }

        public Emitter(Type Attribute) {
            this.Attribute = Attribute;
        }

        /* By default maps properties, but if Attribute isn't null maps fields as well. */
        public virtual Type EmitClass(Type srcType, Type destType, ModuleBuilder moduleBuilder, Dictionary<String, String> dict)
        {
           /* if (Attribute != null)
                Fields.Map(srcObject, destObject, Attribute, dict); */
           return  Properties.EmitClass(srcType, destType, moduleBuilder, dict);
        }

    }

}

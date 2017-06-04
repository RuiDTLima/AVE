using MapperGeneric;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperGeneric {
    public class MappingProperties : Mapping {

        private PropertiesEmitter classEmiter = new PropertiesEmitter();

        /* Auxiliar method to be called without using attribute. */
        public override void Map(object srcObject, object destObject, Dictionary<string, string> dict, Dictionary<string, Func<R>> dictFuncs) {
            Map(srcObject, destObject, null, dict, dictFuncs);
        }
        /* For each property in srcObject maps its value in the corresponding property in destObject depending on attribute. */
        public override void Map(object srcObject, object destObject, Type attr, Dictionary<string, string> dict, Dictionary<string, Func<R>> dictFuncs) {
            Type source = srcObject.GetType();
            /* Get the Class of the mapper. */
            Type mapperClass = classEmiter.EmitClass( source, destObject.GetType(), attr, dict, dictFuncs);
            /* Create an instance of the Class mapper. */
            MappingEmit mapper = (MappingEmit) Activator.CreateInstance(mapperClass);
            /* Maps the srcObject in destObject with the mapper. */
            mapper.Map(srcObject, destObject);
        }
    }
}

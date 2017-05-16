﻿using MapperEmit.Emiter;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperEmit {
    public class MappingFields : Mapping {

        private FieldEmitter classEmiter = new FieldEmitter();

        /* Auxiliar method to be called without using attribute. */
        public override void Map(object srcObject, object destObject, ModuleBuilder mb, Dictionary<String, String> dict) {
            Map(srcObject, destObject, null, mb, dict);
        }

        /* For each field in srcObject maps its value in the corresponding field in destObject depending on attribute. */
        public override void Map(object srcObject, object destObject, Type attr, ModuleBuilder mb, Dictionary<String, String> dict) {
            /* Get the Class of the mapper. */
            Type mapperClass = classEmiter.EmitClass(srcObject.GetType(), destObject.GetType(), attr, mb, dict);
            /* Create an instance of the Class mapper. */
            MappingEmit mapper = (MappingEmit)Activator.CreateInstance(mapperClass);
            /* Maps the srcObject in destObject with the mapper. */
            mapper.Map(srcObject, destObject);
        }
   }
}

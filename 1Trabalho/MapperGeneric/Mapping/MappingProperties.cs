﻿using System;
using System.Collections.Generic;

namespace MapperGeneric {
    public class MappingProperties : Mapping {

        private PropertiesEmitter classEmiter = new PropertiesEmitter();

        /* Auxiliar method to be called without using attribute. */
        public override void Map(object srcObject, object destObject, Dictionary<string, string> dict, Dictionary<string, Func<object>> dictResult) {
            Map(srcObject, destObject, null, dict, dictResult);
        }
        /* For each property in srcObject maps its value in the corresponding property in destObject depending on attribute. */
        public override void Map(object srcObject, object destObject, Type attr, Dictionary<string, string> dict, Dictionary<string, Func<object>> dictResult) {
            /* Create an instance of the Class mapper. */
            IMappingEmit mapper = classEmiter.EmitClass(srcObject.GetType(), destObject.GetType(), attr, dict, dictResult);
            /* Maps the srcObject in destObject with the mapper. */
            mapper.Map(srcObject, destObject);
        }
    }
}
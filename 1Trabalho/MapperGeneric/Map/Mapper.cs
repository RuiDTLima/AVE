using MapperGeneric.Emiter;
using System;
using System.Collections.Generic;

namespace MapperGeneric {
    public class Mapper : IMapper {
        private Type src;
        private Type dest;

        private Mapping mapAtribute;
        private ConstructorEmitter ctorEmitter = new ConstructorEmitter();
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public Mapper(Type source, Type destination) {
            src = source;
            dest = destination;
            mapAtribute = new Mapping();
        }

        /* Verify if it's possible to map the object received in parameters in the destination type,
         * if so creates the new object and maps it. */
        public object Map(object srcObject) {
            if (srcObject == null || !srcObject.GetType().Equals(src))
                return null;
            Type ctorType = ctorEmitter.EmitClass(dest);
            ConstructorEmit ctorEmited = (ConstructorEmit)Activator.CreateInstance(ctorType);
            object destObject = ctorEmited.createInstance(dest);
            mapAtribute.Map(srcObject, destObject, dict);
            return destObject;
        }

        /* Return a new object array that will contain each object from the array received in parameters mapped. */
        public object[] Map(object[] src) {
            object[] ret = (object[]) Array.CreateInstance(dest, src.Length);
            for (int i = 0; i < src.Length; i++){
                ret[i] = Map(src[i]);
            }
            return ret;
        }

        /* Change the current mapAtribute. */
        public IMapper Bind(Mapping m) {
            mapAtribute = m;
            return this;
        }

        /* Add a new entry match of names if it's not already contained in the dict. */
        public IMapper Match(string nameFrom, string nameDest)
        {
            String aux = null;
            if (!dict.TryGetValue(nameFrom, out aux))
                dict.Add(nameFrom, nameDest);
            return this;
        }
    }
}
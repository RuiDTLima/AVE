using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class Mapping
    {
        public virtual Type src { get; set; }

        public virtual Type dest { get; set; }

        protected object destObject;
        protected Dictionary<KeyValuePair<Type, Type>, IMapper> cacheContainer = Cache.cache;
        protected String currentName;
        protected Type currentDestType, currentSrcType;

        public Mapping() { }

        public Mapping(Type source, Type destino)
        {
            src = source;
            dest = destino;
        }

        public virtual object Map(object srcObject, Dictionary<String, String> dict)
        {
            throw new NotImplementedException();
        }

        public object[] Map(object[] srcObject, Dictionary<String, String> dict)
        {
            object[] obj = new object[srcObject.Length];
            for (int i = 0; i < srcObject.Length; ++i)
            {
                obj[i] = Map(srcObject[i], dict);
            }
            return obj;
        }

    }
}

using System;
using System.Collections.Generic;

namespace MapperReflect
{
    public class Mapping
    {
        public virtual Type src { get; set; }

        public virtual Type dest { get; set; }

        public Mapping() { }

        public Mapping(Type source, Type destino)
        {
            src = source;
            dest = destino;
        }

        public virtual object Map(object src, Dictionary<String, String> dict) {
            throw new NotImplementedException();
        }

        public object[] Map(object[] src, Dictionary<String, String> dict)
        {
            object[] obj = new object[src.Length];
            for (int i = 0; i < src.Length; ++i)
            {
                obj[i] = Map(src[i],dict);
            }
            return obj;
        }
    }
}

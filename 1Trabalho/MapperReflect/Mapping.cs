using System;
using System.Collections.Generic;

namespace MapperReflect
{
    class Mapping
    {

        public virtual Type src { get; set; }

        public virtual Type dest { get; set; }

        public virtual object Map(object src, Dictionary<String, String> dict) {
            throw new NotImplementedException();
        }

        public object[] Map(object[] src)
        {
            object[] obj = new object[src.Length];
            for (int i = 0; i < src.Length; ++i)
            {
                obj[i] = Map(src[i]);
            }
            return obj;
        }
    }
}

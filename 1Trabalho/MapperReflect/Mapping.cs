using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperReflect
{
    class Mapping : IMapper
    {
        public virtual Type src { get; set; }

        public virtual Type dest { get; set; }

        public virtual object Map(object src) {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperReflect
{
    class Mapping : IMapper
    {
<<<<<<< HEAD
        [AttributeUsage(AttributeTargets.Property)]
        internal class Properties : Attribute { }

        [AttributeUsage(AttributeTargets.Property)]
        internal class Fields : Attribute {}
        
=======
        public virtual Type src { get; set; }

        public virtual Type dest { get; set; }

        public virtual object Map(object src) {
            throw new NotImplementedException();
        }
>>>>>>> 169a6d4c1706a134836b51e3f3828eb76eec0d1f

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

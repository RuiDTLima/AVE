using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperReflect
{

    class Mapping
    {
        [AttributeUsage(AttributeTargets.Property)]
        internal class Properties : Attribute { }

        [AttributeUsage(AttributeTargets.Property)]
        internal class Fields : Attribute {}
        

    }
}

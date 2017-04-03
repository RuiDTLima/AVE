using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperReflect
{
    interface IMapper
    {
        object Map(object src);

        object[] Map(object[] src);
    }
}

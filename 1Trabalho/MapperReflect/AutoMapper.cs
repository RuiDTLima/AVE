using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperReflect
{

    public class AutoMapper
    {
        public static IMapper Build(Type source, Type destination)
        {
             return new Mapper(source, destination);
        }
    }
}

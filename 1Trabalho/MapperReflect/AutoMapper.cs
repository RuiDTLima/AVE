using System;

namespace MapperReflect
{

    public class AutoMapper
    {
        static IMapper Build(Type source, Type destination)
        {
             return new Mapper(source, destination);
        }
    }
}

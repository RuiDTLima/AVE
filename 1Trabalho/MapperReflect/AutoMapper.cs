using System;

namespace MapperReflect
{
    public class AutoMapper
    {
        public static IMapper Build(Type klassSrc, Type klassDest)
        {
             return new Mapper(klassSrc, klassDest);
        }
    }
}

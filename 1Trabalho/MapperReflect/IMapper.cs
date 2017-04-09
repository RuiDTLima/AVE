using System;

namespace MapperReflect
{
    public interface IMapper
    {
        object Map(object src);

        object[] Map(object[] src);

        IMapper Bind(Mapping m);

        IMapper Match(String nameFrom, String nameDest);
    }
}

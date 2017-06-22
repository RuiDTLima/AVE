using System;

namespace MapperGeneric
{
    public interface IConstructorEmit
    {
        object createInstance(Type type);
    }
}

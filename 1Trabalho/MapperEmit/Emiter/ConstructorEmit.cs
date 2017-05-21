using System;

namespace MapperEmit.Emiter
{
    public interface ConstructorEmit
    {
        object createInstance(Type type);
    }
}

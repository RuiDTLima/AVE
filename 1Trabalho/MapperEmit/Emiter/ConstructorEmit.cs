using System;
using System.Reflection;

namespace MapperEmit.Emiter
{
    public interface ConstructorEmit
    {
        object GetCtor(Type type);
    }
}

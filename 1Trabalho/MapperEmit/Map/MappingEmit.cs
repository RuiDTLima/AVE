using MapperEmit.Emitter;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperEmit
{
    public interface MappingEmit
    {
        void Map(object source, object dest);

    }
}

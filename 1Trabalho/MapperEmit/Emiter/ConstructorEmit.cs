﻿using System;

namespace MapperGeneric.Emiter
{
    public interface ConstructorEmit
    {
        object createInstance(Type type);
    }
}

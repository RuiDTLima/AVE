using System;
using System.Collections.Generic;

namespace MapperReflect
{
    class Cache {
        public static Dictionary<KeyValuePair<Type, Type>, IMapper> cache = new Dictionary<KeyValuePair<Type, Type>, IMapper>();
    }
}

using System;
using System.Collections.Generic;

namespace MapperGeneric {
    class Cache {
        public static Dictionary<KeyValuePair<Type, Type>, IMapper> cache = new Dictionary<KeyValuePair<Type, Type>, IMapper>();
    }
}

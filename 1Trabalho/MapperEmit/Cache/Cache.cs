using System;
using System.Collections.Generic;

namespace MapperEmit {
    class Cache {
        public static Dictionary<KeyValuePair<Type, Type>, IMapper> cache = new Dictionary<KeyValuePair<Type, Type>, IMapper>();
    }
}

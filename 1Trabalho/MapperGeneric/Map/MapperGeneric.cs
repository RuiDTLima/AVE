using System;
using System.Collections.Generic;

namespace MapperGeneric {
    public class Mapper<TSrc, TDest> : Mapper, IMapperGeneric<TSrc, TDest> {
        private Mapping mapAtribute = new Mapping();
        private ConstructorEmitter ctorEmitter = new ConstructorEmitter();
        private Dictionary<string, string> dict = new Dictionary<string, string>();
        private Dictionary<string, object> dictResult = new Dictionary<string, object>();

        public Mapper(Type source, Type destination) : base(source, destination){
        }

        public TDest Map(TSrc src) {
            Type TSrcType = typeof(TSrc);
            if (src == null)
                return default(TDest);
            Type dest = typeof(TDest);
            Type ctorType = ctorEmitter.EmitClass(dest);
            ConstructorEmit ctorEmited = (ConstructorEmit)Activator.CreateInstance(ctorType);
            TDest destObject = (TDest) ctorEmited.createInstance(dest);
            mapAtribute.Map(src, destObject, dict, dictResult);
            return destObject;
        }

        public TDest[] Map(TSrc[] src) {
            TDest[] ret = (TDest[])Array.CreateInstance(typeof(TDest), src.Length);
            for (int i = 0; i < src.Length; i++) {
                ret[i] = Map(src[i]);
            }
            return ret;
        }

        public L Map<L>(TSrc[] src) where L : ICollection<TDest> {
            L x = (L) Activator.CreateInstance(typeof(L));
            for (int i = 0; i < src.Length; i++) {
                x.Add(Map(src[i]));
            }
            return x;
        }

        public IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src)
        {
            foreach (TSrc source in src) {
                yield return Map(source);
            }
        }

        public IMapper For<R>(string nameFrom, Func<object> func) {
            object result;
            if (!dictResult.TryGetValue(nameFrom, out result)) {
                result = func.Invoke();
                dictResult.Add(nameFrom, result);
            }
            return this;
        }
    }
}
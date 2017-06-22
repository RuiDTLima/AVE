using System;
using System.Collections.Generic;

namespace MapperGeneric {
    public class MapperGeneric<TSrc, TDest> : IMapperGeneric<TSrc, TDest> { 
        
        protected Mapping mapAtribute;
        protected ConstructorEmitter ctorEmitter = new ConstructorEmitter();
        protected Dictionary<string, string> namesDictionary = new Dictionary<string, string>();
        protected Dictionary<string, Func<object>> functionsDictionary = new Dictionary<string, Func<object>>();

        public TDest Map(TSrc src) {
            if (src == null)
                return default(TDest);
            Type dest = typeof(TDest);
            IConstructorEmit ctorEmited = ctorEmitter.EmitClass(dest); 
            TDest destObject = (TDest) ctorEmited.createInstance(dest);
            mapAtribute.Map(src, destObject, namesDictionary, functionsDictionary);
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
        
        /* Change the current mapAtribute. */
        public IMapperGeneric<TSrc, TDest> Bind(Mapping m)
        {
            mapAtribute = m;
            return this;
        }

        /* Add a new entry match of names if it's not already contained in the dict. */

        public IMapperGeneric<TSrc, TDest> Match(string nameFrom, string nameDest)
        {
            string aux = null;
            if (!namesDictionary.TryGetValue(nameFrom, out aux))
                namesDictionary.Add(nameFrom, nameDest);
            return this;
        }

        public IMapperGeneric<TSrc, TDest> For<R>(string nameFrom, Func<R> func) where R : class
        {

            if (!functionsDictionary.ContainsKey(nameFrom))
            {
                functionsDictionary.Add(nameFrom, func);
            }
            return this;
        } 
    }
}
using System;
using System.Collections.Generic;

namespace MapperGeneric
{
    public interface IMapperGeneric<TSrc, TDest>
    {
        TDest Map(TSrc src);

        TDest[] Map(TSrc[] src);

        L Map<L>(TSrc[] src) where L : ICollection<TDest>;

        IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src);

        IMapperGeneric<TSrc, TDest> Bind(Mapping m);

        IMapperGeneric<TSrc, TDest> Match(string nameFrom, string nameDest);

        IMapperGeneric<TSrc, TDest> For<R>(string nameFrom, Func<R> func) where R : class;
    }
}
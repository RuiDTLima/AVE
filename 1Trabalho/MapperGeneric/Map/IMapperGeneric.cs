using System.Collections.Generic;

namespace MapperGeneric
{
    public interface IMapperGeneric<TSrc, TDest> : IMapper
    {
        TDest Map(TSrc src);
        TDest[] Map(TSrc[] src);
        IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src);
    }
}
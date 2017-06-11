using System;
namespace MapperGeneric {
    public interface IMapper {
        object Map(object src);

        object[] Map(object[] src);

        IMapper Bind(Mapping m);

        IMapper Match(string nameFrom, string nameDest);

        IMapper For<R>(string nameFrom, Func<R> func) where R : class;
    }
}
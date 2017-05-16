using System;
namespace MapperEmit {
    public interface IMapper {
        object Map(object src);

        object[] Map(object[] src);

        IMapper Bind(Emiter.Emitter m);

        IMapper Match(String nameFrom, String nameDest);
    }
}
using System.Collections.Generic;

namespace MapperReflect
{
    class Mapper : IMapper
    {
        private Type src { get; set; }

        private Type dest { get; set; }

        private Mapping mapAtribute { get; set; }

        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public Mapper(Type source, Type destination)
        {
            src = source;
            dest = destination;
        }

        //Falta aplicar a parte do bind e do match.
        public object Map(object src)
        { 
            return mapAtribute.Map(src);
        }

        public object[] Map(object[] src)
        {
            return mapAtribute.Map(src);
        }

        public Mapper Bind(Mapping m)
        {
            mapAtribute = m;
            return this;
        }

        public Mapper Match(string nameFrom, string nameDest)
        {
            dict.Add(nameFrom, nameDest);
            return this;
        }

    }
}

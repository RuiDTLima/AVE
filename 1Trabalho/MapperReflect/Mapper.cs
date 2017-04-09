using System;
using System.Collections.Generic;

namespace MapperReflect
{
    public class Mapper : IMapper
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
            //if (src != null)
             return mapAtribute.Map(src, dict);
           // return null;
        }

        public object[] Map(object[] src)
        {
            return mapAtribute.Map(src, dict);
        }

        public IMapper Bind(Mapping m)
        {
            mapAtribute = m;
            mapAtribute.src = this.src;
            mapAtribute.dest = this.dest;
            return this;
        }

        public IMapper Match(string nameFrom, string nameDest)
        {
            dict.Add(nameFrom, nameDest);
            return this;
        }

    }
}

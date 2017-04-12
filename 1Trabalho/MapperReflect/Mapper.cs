using System;
using System.Collections.Generic;
using System.Reflection;

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

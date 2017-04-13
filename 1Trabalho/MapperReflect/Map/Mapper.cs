using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class Mapper : IMapper
    {
        private Type src;
       
        private Type dest; 

        private Mapping mapAtribute;

        private Dictionary<string, string> dict = new Dictionary<string, string>();


        public Mapper(Type source, Type destination) {
            src = source;
            dest = destination;
            mapAtribute = new MappingProperties();
        }

        public object Map(object src) {
            return mapAtribute.Map(src, this.src, this.dest, dict);
        }

        public object[] Map(object[] src) {
            return mapAtribute.Map(src, this.src, this.dest, dict);
        }

        public IMapper Bind(Mapping m) {
            mapAtribute = m;
            return this;
        }

        public IMapper Match(string nameFrom, string nameDest) {
            dict.Add(nameFrom, nameDest);
            return this;
        }
    }
}
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace MapperReflect
{
    class Mapper : IMapper
    {
        private Type src { get; set; }

        private Type dest { get; set; }

        public Mapping mapAtribute { get; private set; }

        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public Mapper(Type source, Type destination)
        {
            src = source;
            dest = destination;
        }

        //Falta aplicar a parte do bind e do match.
        public object Map(object src)
        {
            if (!src.GetType().Equals(this.src))
                return null;
            ArrayList p = new ArrayList();
            PropertyInfo[] srcProperties = src.GetType().GetProperties();
            object aux = Activator.CreateInstance(dest);
            PropertyInfo[] destProperties = dest.GetProperties();
            PropertyInfo destino, origem;

            for (int i = 0; i < destProperties.Length; i++)
            {
                for (int j = 0; j < srcProperties.Length; j++) { 
                    destino = destProperties[i];
                    origem = srcProperties[j];
                    if (destino.GetType().Equals(origem.GetType()) && destino.Name.Equals(origem.Name))
                        destino.SetValue(aux, origem.GetValue(src));
                }
            }
            return aux;
        }

        public object[] Map(object[] src)
        {
            object[] obj = new object[src.Length];
            for (int i = 0; i < src.Length; ++i) {
                obj[i] = Map(src[i]);
            }
            return obj;
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

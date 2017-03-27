using System;
using System.Reflection;

namespace MapperReflect
{
    class Mapper : IMapper
    {
        private Type src { get; set; }

        private Type dest { get; set; }

        public Mapper(Type source, Type destination)
        {
            src = source;
            dest = destination;
        }


        public object Map(object src)
        {
            if (!src.GetType().Equals(this.src))
                return null;
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
            throw new NotImplementedException();
        }
    }
}

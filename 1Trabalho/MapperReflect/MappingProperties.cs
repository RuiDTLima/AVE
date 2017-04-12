using System;
using System.Reflection;
using System.Collections.Generic;

namespace MapperReflect
{
    class MappingProperties : Mapping
    {
        public override object Map(object src)
        {
            if (!src.GetType().Equals(this.src))
                return null;
            PropertyInfo[] srcProperties = src.GetType().GetProperties();
            object aux = Activator.CreateInstance(dest);
            PropertyInfo[] destProperties = dest.GetProperties();
            PropertyInfo destino, origem;

            for (int i = 0; i < destProperties.Length; i++)
            {
                for (int j = 0; j < srcProperties.Length; j++)
                {
                    destino = destProperties[i];
                    origem = srcProperties[j];
                    if (destino.GetType().Equals(origem.GetType()) && destino.Name.Equals(origem.Name))
                        destino.SetValue(aux, origem.GetValue(src));
                }
            }
            return aux;
        }
    }
}

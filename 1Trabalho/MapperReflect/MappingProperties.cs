using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingProperties : Mapping
    {
        //TODO finish
        public override object Map(object src, Dictionary<String, String> dict)
        {
            if (!src.GetType().Equals(this.src))
                return null;
            PropertyInfo[] srcProperties = src.GetType().GetProperties();
            object aux = Activator.CreateInstance(dest);
            PropertyInfo[] destProperties = dest.GetProperties();
            PropertyInfo destino, origem;
            String temp;

            for (int i = 0; i < srcProperties.Length; i++)
            {
                origem = srcProperties[i];
                dict.TryGetValue(origem.Name, out temp);
                destino = temp == null ? dest.GetProperty(origem.Name) : destino = dest.GetProperty(temp);

                if (destino.GetType().Equals(origem.GetType()))
                    destino.SetValue(aux, origem.GetValue(src));
            }
          /*  for (int i = 0; i < destProperties.Length; i++)
            {
                destino = destProperties[i];
                for (int j = 0; j < srcProperties.Length; j++)
                {
                   ;
                    if (destino.GetType().Equals(origem.GetType()) && destino.Name.Equals(origem.Name) || dict.TryGetValue(origem.Name,out temp) && temp.Equals(destino.Name))
                        destino.SetValue(aux, origem.GetValue(src));
                }
            }*/
            return aux;
        }
    }
}

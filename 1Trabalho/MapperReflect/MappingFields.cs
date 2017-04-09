using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    class MappingFields : Mapping
    {
        public override object Map(object src, Dictionary<String, String> dict)
        {
            if (!src.GetType().Equals(this.src))
                return null;
            FieldInfo[] srcFields = src.GetType().GetFields();
            object aux = Activator.CreateInstance(dest);
            FieldInfo[] destFields = dest.GetFields();
            FieldInfo destino, origem;

            for (int i = 0; i < destFields.Length; i++)
            {
                for (int j = 0; j < srcFields.Length; j++)
                {
                    destino = destFields[i];
                    origem = srcFields[j];
                    if (destino.GetType().Equals(origem.GetType()) && destino.Name.Equals(origem.Name))
                        destino.SetValue(aux, origem.GetValue(src));
                }
            }
            return aux;
        }
    }
}

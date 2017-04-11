using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingToMap : Mapping
    {
        public MappingToMap()
        {

        }

        public MappingToMap(Type source, Type destino) : base(source, destino)
        {
        }

        public override object Map(object src, Dictionary<String, String> dict)
        {
            if (!src.GetType().Equals(this.src))
                return null;
            MemberInfo[] srcMembers = src.GetType().GetMembers();
            ArrayList srcAttributes = new ArrayList();
            for (int i = 0; i < srcMembers.Length; i++)
            {
                if (srcMembers[i].IsDefined(this.src))
                {
                    srcAttributes.Add(srcAttributes[i]);
                }
            }
            object aux = Activator.CreateInstance(dest);
            MemberInfo[] destMembers = dest.GetType().GetMembers();
            MemberInfo destino, origem;

            for (int i = 0; i < destMembers.Length; i++)
            {
                for (int j = 0; j < srcAttributes.Count; j++)
                {
                    destino = destMembers[i];
                    origem = (MemberInfo)srcAttributes[j];
                    if (destino.GetType().Equals(origem.GetType()) && destino.Name.Equals(origem.Name))
                    {
                       //destino.GetType().GetMethod("SetValue").Invoke(aux, new object[] { origem.GetType().GetMethod("GetValue").Invoke(src, null) });
                        PropertyInfo pi = origem as PropertyInfo;
                        if (pi != null)
                        {
                            pi.SetValue(aux, origem);
                        } else
                        {
                            ((FieldInfo)destino).SetValue(aux, ((FieldInfo)origem).GetValue(src));
                        }
                    }
                }
            }
            return aux;
        }
    }
}
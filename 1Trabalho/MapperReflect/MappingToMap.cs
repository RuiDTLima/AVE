using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace MapperReflect
{
    class MappingToMap : Mapping
    {
        public override object Map(object src)
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
                      /* if (destino.GetType() == typeof(PropertyInfo))
                        {
                            ((PropertyInfo)destino).SetValue(aux, ((PropertyInfo)origem).GetValue(src));
                        }
                       else if (destino.GetType() == typeof(FieldInfo))
                        {
                            ((FieldInfo)destino).SetValue(aux, ((FieldInfo)origem).GetValue(src));
                        }*/
                    }
                        //destino = origem;
                        //destino.SetValue(aux, origem.GetValue(src));
                }
            }
            return aux;
        }
    }
}
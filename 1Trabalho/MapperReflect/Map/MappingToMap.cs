using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class MappingToMap : Mapping
    {
       
        public object Map(object srcObject, Type src, Type dest, Type attr, Dictionary<String, String> dict) {
            MemberInfo[] srcMembers, destMembers = dest.GetMembers();
            MemberInfo destiny, origin;
            String currentName;
            Type currentDestType, currentSrcType;

            object destObject = init(srcObject, src, dest, out srcMembers);

            for (int i = 0; i < srcMembers.Length; i++) {
                if (!srcMembers[i].IsDefined(attr)) continue;

                origin = srcMembers[i];

                /* Get current destination Member. */
                dict.TryGetValue(origin.Name, out currentName);

                destiny = (MemberInfo) (currentName == null ?
                     
                if (destiny == null) continue; 

                currentDestType = destiny.GetType();
                currentSrcType = origin.GetType();

                if (destiny.GetType().Equals(origin.GetType()))  { 
                        PropertyInfo pi = origin as PropertyInfo;
                        if (pi != null) {
                            pi.SetValue(destObject, origin);
                        } else {
                            ((FieldInfo)destiny).SetValue(destObject, ((FieldInfo)origin).GetValue(src));
                        }
                    } else if (!currentSrcType.IsValueType && !currentDestType.IsValueType && !currentDestType.IsSubclassOf(currentSrcType)) {
                        IMapper cache;
                        KeyValuePair<Type, Type> typePair = new KeyValuePair<Type, Type>(currentSrcType, currentDestType);
                        cacheContainer.TryGetValue(typePair, out cache);

                        if (cache == null) {
                            cache = AutoMapper.Build(currentSrcType, currentDestType);
                            cacheContainer.Add(typePair, cache);
                        }
                        cache.Map(origin);
                }
            }

            return destObject;
        }

        public object init(object srcObject, Type src, Type dest, out MemberInfo[] srcMembers) {
            srcMembers = null;
            if (!srcObject.GetType().Equals(src))
                return null;

            srcMembers = srcObject.GetType().GetMembers();
            if (dest.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(dest);
            return getAvailableConstructor(srcMembers, dest).Invoke(new Object[srcMembers.Length]);
        }

        private ConstructorInfo getAvailableConstructor(MemberInfo[] srcMembers, Type dest) {
            int size = srcMembers.Length;
            Type[] memberTypes = new Type[size];
            for (int i = 0; i < size; ++i)
                memberTypes[i] = srcMembers[i].GetType();
            return dest.GetConstructor(memberTypes);
        }
    }
}
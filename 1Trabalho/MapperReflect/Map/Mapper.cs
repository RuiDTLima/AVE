using System;
using System.Collections;
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
            mapAtribute = new Mapping();
        }

        public object Map(object srcObject) {
            if (srcObject == null || !srcObject.GetType().Equals(src))
                return null;
            object destObject = init(dest, getValidMembers(src.GetMembers()));
            mapAtribute.Map(srcObject, destObject, dict);
            return destObject;
        }

        public object[] Map(object[] src) {
            object[] ret = (object[]) Array.CreateInstance(dest, src.Length);
            for (int i = 0; i < src.Length; i++){
                ret[i] = Map(src[i]);
            }
            return ret;
        }

        public IMapper Bind(Mapping m) {
            mapAtribute = m;
            return this;
        }

        public IMapper Match(string nameFrom, string nameDest) {
            String aux = null;
            if(!dict.TryGetValue(nameFrom, out aux))
                dict.Add(nameFrom, nameDest);
            return this;
        }

        private object init(Type dest, ArrayList srcMembers)
        {
            if (dest.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(dest);
            return getAvailableConstructor(srcMembers, dest).Invoke(new Object[srcMembers.Count]);
        }

        private ConstructorInfo getAvailableConstructor(ArrayList srcMembers, Type dest) {
            int size = srcMembers.Count;
            Type[] memberTypes = new Type[size];
            for (int i = 0; i < size; ++i)
                memberTypes[i] = srcMembers[i].GetType();
            return dest.GetConstructor(memberTypes);
        }

        private ArrayList getValidMembers(MemberInfo[] members) {
            ArrayList validMembers = new ArrayList();
            for (int i = 0; i < members.Length; i++){
                MemberInfo curr = members[i];
                if (isValidMember(curr)) validMembers.Add(curr);
            }
            return validMembers;
        }

        private bool isValidMember(MemberInfo curr)
        {
            //return curr.GetType() == typeof(PropertyInfo) || curr.GetType() == typeof(FieldInfo);
            PropertyInfo pi = curr as PropertyInfo;
            if (pi != null) return true;

            FieldInfo fi = curr as FieldInfo;
            if (fi != null) return true;
            return false;
        }
    }
}
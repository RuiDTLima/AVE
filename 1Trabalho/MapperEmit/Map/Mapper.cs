using MapperEmit.Emiter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperEmit {
    public class Mapper : IMapper {
        private Type src;
        private Type dest;
        
        private Mapping mapAtribute;
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public Mapper(Type source, Type destination) {
            src = source;
            dest = destination;
        }

        /* Verify if it's possible to map the object received in parameters in the destination type,
         * if so creates the new object and maps it. */
        public object Map(object srcObject) {
            if (srcObject == null || !srcObject.GetType().Equals(src))
                return null;
            object destObject = init(dest, getValidMembers(src.GetMembers()));
            mapAtribute.Map(srcObject, destObject, dict);
            return destObject;
        }

        /* Return a new object array that will contain each object from the array received in parameters mapped. */
        public object[] Map(object[] src) {
            object[] ret = (object[]) Array.CreateInstance(dest, src.Length);
            for (int i = 0; i < src.Length; i++){
                ret[i] = Map(src[i]);
            }
            return ret;
        }

        /* Change the current mapAtribute. */
        public IMapper Bind(Mapping m) {
            mapAtribute = m;
            return this;
        }

        /* Add a new entry match of names if it's not already contained in the dict. */
        public IMapper Match(string nameFrom, string nameDest) {
            String aux = null;
            if(!dict.TryGetValue(nameFrom, out aux))
                dict.Add(nameFrom, nameDest);
            return this;
        }

        /* Get a new object using the empty parameters constructor or the constructor that receives parameters
         * with the types contained in the ArrayList. */
        private object init(Type dest, ArrayList srcMembers) {
            if (dest.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(dest);
            return getAvailableConstructor(srcMembers, dest).Invoke(new object[srcMembers.Count]);
        }

        /* Get the constructor that receveives parameters which types are contained in ArrayList. */
        private ConstructorInfo getAvailableConstructor(ArrayList srcMembers, Type dest) {
            int size = srcMembers.Count;
            Type[] memberTypes = new Type[size];
            for (int i = 0; i < size; ++i)
                memberTypes[i] = srcMembers[i].GetType();
            return dest.GetConstructor(memberTypes);
        }

        /* Get the ArrayList of valid members contained in members. */
        private ArrayList getValidMembers(MemberInfo[] members) {
            ArrayList validMembers = new ArrayList();
            for (int i = 0; i < members.Length; i++){
                MemberInfo curr = members[i];
                if (isValidMember(curr)) validMembers.Add(curr);
            }
            return validMembers;
        }

        /* Verifies if curr is a PropertyInfo or a FieldInfo. */
        private bool isValidMember(MemberInfo curr) {
            
            PropertyInfo pi = curr as PropertyInfo;
            if (pi != null) return true;

            FieldInfo fi = curr as FieldInfo;
            if (fi != null) return true;
            return false;
        }
    }
}
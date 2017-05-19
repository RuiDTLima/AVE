using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperEmit.Emiter
{
    class ConstructorEmitter : Emitter
    {
        public override Type EmitClass(Type srcType, Type destType) {
            Type emittedClass;
            /* Verify if the class to emit already exists and returns it. */
            if (IsInCache(srcType, destType, out emittedClass))
                return emittedClass;
            AssemblyName aName = new AssemblyName("ConstructorAssembly");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType("ConstructorMapping" + srcType.Name + "To" + destType.Name,
                                                               TypeAttributes.Public);

            /* Define that the emittied class is a Mapping */
            typeBuilder.AddInterfaceImplementation(typeof(ConstructorEmit));
            /* Arrange the method that would look like public void Map(object srcObject, object destObject Type attribute, Dictionary<string, string> dic) */
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetCtor", MethodAttributes.Public | MethodAttributes.Virtual,
                                                                   typeof(object), new Type[] { typeof(Type) });

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ConstructorInfo ctorInfo = destType.GetConstructor(Type.EmptyTypes);
            if (ctorInfo == null)
            {
                int count = getNumberValidMembers(srcType.GetMembers());
                while(count > 0)
                {
                    ilGenerator.Emit(OpCodes.Ldnull);
                    count--;
                }
                ctorInfo = getAvailableConstructor(getValidMembers(srcType.GetMembers()), destType);
            }

            ilGenerator.Emit(OpCodes.Newobj, ctorInfo);
            ilGenerator.Emit(OpCodes.Ret);
            emittedClass = typeBuilder.CreateType();
            ab.Save("ConstructorAssembly.dll");

            addToCache(srcType, destType, emittedClass);
            return emittedClass;
        }

        public override Type EmitClass(Type srcType, Type destType, Type attr, Dictionary<string, string> dict) {
            throw new NotImplementedException();
        }

        /* Get the constructor that receveives parameters which types are contained in ArrayList. */
        private ConstructorInfo getAvailableConstructor(ArrayList srcMembers, Type dest)
        {
            int size = srcMembers.Count;
            Type[] memberTypes = new Type[size];
            for (int i = 0; i < size; ++i)
                memberTypes[i] = srcMembers[i].GetType();
            return dest.GetConstructor(memberTypes);
        }

        /* Get the ArrayList of valid members contained in members. */
        private int getNumberValidMembers(MemberInfo[] members)
        {
            int count = 0;
            for (int i = 0; i < members.Length; i++)
            {
                MemberInfo curr = members[i];
                if (isValidMember(curr)) count++;
            }
            return count;
        }

        /* Verifies if curr is a PropertyInfo or a FieldInfo. */
        private bool isValidMember(MemberInfo curr)
        {

            PropertyInfo pi = curr as PropertyInfo;
            if (pi != null) return true;

            FieldInfo fi = curr as FieldInfo;
            if (fi != null) return true;
            return false;
        }

        /* Get the ArrayList of valid members contained in members. */
        private ArrayList getValidMembers(MemberInfo[] members)
        {
            ArrayList validMembers = new ArrayList();
            for (int i = 0; i < members.Length; i++)
            {
                MemberInfo curr = members[i];
                if (isValidMember(curr)) validMembers.Add(curr);
            }
            return validMembers;
        }
    }
}
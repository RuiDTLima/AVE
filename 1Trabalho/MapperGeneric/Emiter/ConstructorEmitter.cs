﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperGeneric
{
    public class ConstructorEmitter : Emitter
    {
        private Dictionary<Type, IConstructorEmit> emittedConstructors = new Dictionary<Type, IConstructorEmit>();

        public override IConstructorEmit EmitClass(Type destType) {
            IConstructorEmit emittedClass;
            /* Verify if the class to emit already exists and returns it. */
            if (IsInCache(destType, out emittedClass))
                return emittedClass;
            AssemblyName aName = new AssemblyName("ConstructorAssembly");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType("ConstructorMapping" + destType.Name,
                                                               TypeAttributes.Public);

            /* Define that the emittied class is a Mapping */
            typeBuilder.AddInterfaceImplementation(typeof(IConstructorEmit));
            /* Arrange the method that would look like public void Map(object srcObject, object destObject Type attribute, Dictionary<string, string> dic) */
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("createInstance", MethodAttributes.Public | MethodAttributes.Virtual,
                                                                   typeof(object), new Type[] { typeof(Type) });

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ConstructorInfo ctorInfo = destType.GetConstructor(Type.EmptyTypes);
            /* If the constructor with no parameters is avaible only needs to create it otherwise needs to fill the parameters that it receives with null values. */
            if (ctorInfo == null)
            {
                /* Get a constructor with parameters. */
                ctorInfo = destType.GetConstructors()[0];
                int count = ctorInfo.GetParameters().Length; 

                /* For each parameter must fill with value null. */
                while (count > 0)
                {
                    ilGenerator.Emit(OpCodes.Ldnull);
                    count--;
                }
            }

            ilGenerator.Emit(OpCodes.Newobj, ctorInfo); /* Create the new object to return. */
            ilGenerator.Emit(OpCodes.Ret);

            Type emittedClassType = typeBuilder.CreateType();
            ab.Save("ConstructorAssembly.dll");
            emittedClass = (IConstructorEmit)Activator.CreateInstance(emittedClassType);
            addToCache(destType, emittedClass);
            return emittedClass;
        }

        private bool IsInCache(Type Type, out IConstructorEmit emittedConstructor)
        {
            return emittedConstructors.TryGetValue(Type, out emittedConstructor);
        }

        private void addToCache(Type type, IConstructorEmit emittedConstructor)
        {
            emittedConstructors.Add(type, emittedConstructor);
        }

        public override IMappingEmit EmitClass(Type srcType, Type destType, Type attr, Dictionary<string, string> dict, Dictionary<string, Func<object>> dictResult) {
            throw new NotImplementedException();
        }
    }
}
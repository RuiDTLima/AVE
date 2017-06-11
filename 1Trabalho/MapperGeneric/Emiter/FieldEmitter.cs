using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MapperGeneric
{
    public class FieldEmitter : Emitter
    {
        public override ConstructorEmit EmitClass(Type destType)
        {
            throw new NotImplementedException();
        }

        public override MappingEmit EmitClass(Type srcType, Type destType, Type attr, Dictionary<string, string> dict, Dictionary<string, Func<object>> dictResult)
        {
            MappingEmit emittedClass;

            /* Verify if the class to emit already exists and returns it. */
            if (IsInCache(srcType, destType, attr, out emittedClass))
                return emittedClass;

            AssemblyName aName = new AssemblyName("MappingAssembly");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType("FieldsMapping" + srcType.Name + "To" + destType.Name,
                                                               TypeAttributes.Public);
            /* Define that the emittied class is a Mapping */
            typeBuilder.AddInterfaceImplementation(typeof(MappingEmit));
            
            FieldBuilder fbArray = typeBuilder.DefineField(
                "values",
                typeof(Func<object>[]),
                FieldAttributes.Private);

            Type[] parametersType = { typeof(Func<object>[]) };

            /* Define the constructor with Func<object> array as a parameter */
            ConstructorBuilder ctor1 = typeBuilder.DefineConstructor(
                MethodAttributes.Public, 
                CallingConventions.Standard, 
                parametersType);

            ILGenerator ctor1IL = ctor1.GetILGenerator();

            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Call,
                typeof(object).GetConstructor(Type.EmptyTypes));

            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_1);
            ctor1IL.Emit(OpCodes.Stfld, fbArray);
            ctor1IL.Emit(OpCodes.Ret);
            
            /* Arrange the method that would look like public void Map(object srcObject, object destObject Type attribute, Dictionary<string, string> dic) */
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Map", MethodAttributes.Public | MethodAttributes.Virtual,
                                                                   typeof(void), new Type[] { typeof(object), typeof(object) });

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();

            FieldInfo[] srcFields = srcType.GetFields();

            FieldInfo destiny, origin;
            string currentName;
            Func<object> func;
            Type currentDestType, currentSrcType;
            List<Func<object>> values = new List<Func<object>>();
            int idx = 0;
            
            /* Verify if source type is an struct or a class, because in case of beeing a struct, it must save the reference and not the object,
             * for this its used the local stack variable 0 to save either the reference or the object and the local stack variable 1 its used as
             * an auxiliar variable. Must do for source object and destination object.
             * In case of beeing a struct type the object comes in a box form. Must do unbox operation.
             * In case of beeing a class type the object needs to be casted to its real type. Must do cast operation. 
             */

            /* Procedure to deal with source object. */
            if (AutoMapper.IsStructType(srcType)) {
                ilGenerator.Emit(OpCodes.Ldarg_1); /* Load the source object into evaluation stack. */
                ilGenerator.Emit(OpCodes.Unbox_Any, srcType); /* Unbox the source object and gets the object into evaluation stack. */
                ilGenerator.DeclareLocal(typeof(Pointer)); /* Determine that the local stack variable 0 is of type Pointer, to accept adresses. */
                ilGenerator.DeclareLocal(srcType); /* Determine that the local stack variable 1 is of type of the source object. */
                ilGenerator.Emit(OpCodes.Stloc_1); /* Store the object that resulted from the operation unbox before into local stack variable 1. */
                ilGenerator.Emit(OpCodes.Ldloca, 1); /* Load the adress of the local stack variable 1. */
                ilGenerator.Emit(OpCodes.Stloc_0); /* Store the adress into local stack variable 0. */
            }
            else {
                ilGenerator.DeclareLocal(srcType);/* Determine that the local stack variable 0 is of type of the source object. */
                ilGenerator.DeclareLocal(typeof(Pointer));/* Determine that the local stack variable 0 is of type Pointer, to accept adresses. */
                ilGenerator.Emit(OpCodes.Ldarg_1);/* Load the source object into evaluation stack. */
                ilGenerator.Emit(OpCodes.Castclass, srcType); /* Cast the source object to his real type. */
                ilGenerator.Emit(OpCodes.Stloc_0);/* Store the object into local stack variable 0. */
            }

            /* Procedure to deal with destination object. */
            if (AutoMapper.IsStructType(destType)) {
                ilGenerator.Emit(OpCodes.Ldarg_2);
                ilGenerator.Emit(OpCodes.Unbox_Any, destType);
                ilGenerator.DeclareLocal(typeof(Pointer));
                ilGenerator.DeclareLocal(destType);
                ilGenerator.Emit(OpCodes.Stloc_3);
                ilGenerator.Emit(OpCodes.Ldloca, 3);
                ilGenerator.Emit(OpCodes.Stloc_2);
            }
            else {
                ilGenerator.DeclareLocal(destType);
                ilGenerator.DeclareLocal(typeof(Pointer));
                ilGenerator.Emit(OpCodes.Ldarg_2);
                ilGenerator.Emit(OpCodes.Castclass, destType);
                ilGenerator.Emit(OpCodes.Stloc_2);
            }

            Type mapperType = typeof(IMapper);

            ilGenerator.DeclareLocal(mapperType);  /* Determine that the local stack variable 4 is of type Imapper. */
            ilGenerator.DeclareLocal(typeof(object)); /* Determine that the local stack variable 5 is of type object. */
            /* For each source field map it's corresponding field in destination. */
            for (int i = 0; i < srcFields.Length; i++) {
                origin = srcFields[i];
                if (attr != null && !origin.IsDefined(attr))
                    continue;

                /* Gets the corresponding destiny field. */
                if (!dict.TryGetValue(origin.Name, out currentName))
                    currentName = origin.Name;

                destiny = destType.GetField(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (destiny == null || (attr != null && !destiny.IsDefined(attr)))
                    continue;

                currentDestType = destiny.FieldType;
                currentSrcType = origin.FieldType;

                if (dictResult.TryGetValue(currentName, out func)) {
                    values.Add(func);
                    ilGenerator.Emit(OpCodes.Ldloc_2); /* Get destination object into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, fbArray);/*descritor do campo que tem referencia para o array de valores*/
                    ilGenerator.Emit(OpCodes.Ldc_I4_S, idx++);
                    ilGenerator.Emit(OpCodes.Ldelem, typeof(Func<object>));

                    ilGenerator.Emit(OpCodes.Callvirt, typeof(Func<object>).GetMethod("Invoke"));
                    ilGenerator.Emit(OpCodes.Castclass, currentDestType);

                    ilGenerator.Emit(OpCodes.Stfld, destiny); /* Store the value in destination field info. */
                    continue;
                }

                 /* If source and destination type are equal then set destination with source value
                  * else asks for a new mapper and tries to set its value
                  */
                if (currentDestType.Equals(currentSrcType)) {
                    /* As the types are equal, it is needed to get the source object value to put it on destination. */
                    ilGenerator.Emit(OpCodes.Ldloc_2); /* Get destination object into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Ldloc_0); /* Get source object into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Ldfld, origin); /* Obtain the value of the field into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Stfld, destiny); /* Store the value in destination field info. */
                }
                else {
                    /* As the types are different its needed to get a new mapper to map it's values. 
                     * The first thing to do is to get a new mapper of these types, so its needed to call
                     * AutoMapper method build to get the mapper.
                     * Once the map is returned its needed to call method bind for properties and map to affect all properties,
                     * And once this is done, its needed to call method bind once again but this time for fields and map to affect all fields,
                     * So this way the destination object has the same value either in properties and fields than the source object.
                     * As the method map returns an object in the end we have 2 different objects, one with the properties mapped and other with the fields mapped,
                     * Therefore its needed to call the method joindata to join both information into one object.
                     */

                    /* Get the Mapper by requesting it to Autommaper passing the 2 types. */
                    ilGenerator.Emit(OpCodes.Ldtoken, currentSrcType); /* Load the handle that contains the source field type into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle")); /* Call method GetTypeFrom handle to get the type of the handle that is in evaluation stack. */
                    ilGenerator.Emit(OpCodes.Ldtoken, currentDestType); /* Load the handle that contains the destination field type into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle")); /* Call method GetTypeFrom handle to get the type of the handle that is in evaluation stack. */
                    ilGenerator.Emit(OpCodes.Call, typeof(AutoMapper).GetMethod("Build", new Type[] { typeof(Type), typeof(Type) })); /* Call the method build from automapper to get the mapper. */
      
                   /* Must verify if the mapper returned by the automapper isn't null.
                    * Its needed to duplicate the mapper and stores it into local stack because the method shall reach the end with
                    * The evaluation stack empty.  
                    */
                    ilGenerator.Emit(OpCodes.Dup); /* Duplicates the value that is in evaluation stack, in this case the Mapper. */
                    ilGenerator.Emit(OpCodes.Stloc, 4); /* Save the mapper into local stack variable 4. */
                    Label failed3 = ilGenerator.DefineLabel(); /* Declare the label to jump if the verification is false.  */
                    ilGenerator.Emit(OpCodes.Brfalse, failed3); /* Verify if the mapper is null, if so jump to label failed. */

                    /* As it was verified that the mapper isn't null, it is now possible to map the fields and the properties. */
                    ilGenerator.Emit(OpCodes.Ldloc, 4); /* Load the mapper into evaluation stack */
                    ilGenerator.Emit(OpCodes.Call, typeof(Mapping).GetProperty("Fields").GetGetMethod());  /* Load the value Mapping.Fields to be the parameter of method bind. */
                    ilGenerator.Emit(OpCodes.Callvirt, mapperType.GetMethod("Bind")); /* Call the method Bind from the mapper. */
                    ilGenerator.Emit(OpCodes.Dup); /* Duplicate the mapper so once mapping fields its done, the mapper is contained in evaluation stack and its not needed to load it again. */

                    ilGenerator.Emit(OpCodes.Ldloc_0); /* Get source object into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Ldfld, origin); /* Obtain the value of the field into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Callvirt, mapperType.GetMethod("Map", new Type[] { typeof(object) })); /* Call method map of mapper to map the fields. */

                    ilGenerator.Emit(OpCodes.Stloc, 5);  /* Store object returned by previous map call.*/

                    ilGenerator.Emit(OpCodes.Call, typeof(Mapping).GetProperty("Properties").GetGetMethod()); /* Load the value Mapping.Properties to be the parameter of method bind. */
                    ilGenerator.Emit(OpCodes.Callvirt, mapperType.GetMethod("Bind")); /* Call the method Bind from the mapper. */

                    ilGenerator.Emit(OpCodes.Ldloc_0); /* Get source object into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Ldfld, origin); /* Obtain the value of the field into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Callvirt, mapperType.GetMethod("Map", new Type[] { typeof(object) })); /* Call method map of mapper to map the properties. */
                    ilGenerator.Emit(OpCodes.Ldloc, 5); /* Load the object returned by the mapping of fields. */
                    ilGenerator.Emit(OpCodes.Call, typeof(Emitter).GetMethod("joinData")); /* Call method join data with the 2 objects as parameters contained in evaluation stack. */
                    ilGenerator.Emit(OpCodes.Stloc, 5);/* Save the object with everything mapped into local stack variable 5. */
                    ilGenerator.Emit(OpCodes.Ldloc_2);/* Load the destination object into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Ldloc, 5);/* Load the object with everything mapped into evaluation stack. */
                    ilGenerator.Emit(OpCodes.Castclass, currentDestType);/* Cast it to its real type. */
                    ilGenerator.Emit(OpCodes.Stfld, destiny); /* Store the value in destination field info. */
                    ilGenerator.MarkLabel(failed3);
                }
            }

            ilGenerator.Emit(OpCodes.Ret); /* Return. */
            Type emittedClassType = typeBuilder.CreateType();
            ab.Save("MappingAssembly.dll");

            MappingEmit instance = (MappingEmit)Activator.CreateInstance(emittedClassType, new object[] { values.ToArray() });
            addToCache(srcType, destType, attr, instance);

            return instance;
        }
    }
}
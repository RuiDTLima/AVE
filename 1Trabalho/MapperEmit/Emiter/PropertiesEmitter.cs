using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

namespace MapperEmit.Emiter
{
    public class PropertiesEmitter : Emitter {

        public override Type EmitClass(Type srcType, Type destType, Type attr, Dictionary<string, string> dict){
            Type emittedClass;
            /* Verify if the class to emit already exists and returns it. */
            if(IsInCache(srcType, destType, attr, out emittedClass))
                return emittedClass;

            AssemblyName aName = new AssemblyName("MappingAssembly");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType("PropertyMapping" + srcType.Name + "To" + destType.Name,
                                                               TypeAttributes.Public);
            /* Define that the emittied class is a Mapping */
            typeBuilder.AddInterfaceImplementation(typeof(MappingEmit));
            /* Arrange the method that would look like public void Map(object srcObject, object destObject Type attribute, Dictionary<string, string> dic) */
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Map", MethodAttributes.Public | MethodAttributes.Virtual,
                                                                   typeof(void), new Type[] { typeof(object), typeof(object)});

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();

            PropertyInfo[] srcProperties = srcType.GetProperties();

            PropertyInfo destiny, origin;
            string currentName;
            Type currentDestType, currentSrcType;
            /* if( automapper.IsStructType(srcobject))
             * unbox_any
             * else castclass
             *
             * if(automapper.IsStructType(destobject))
             * unbox_any
             * else cast class
             */


            //Get the source object and dest object to cast from object to their types. 
            /* public void Map(object src, object dest) */

            
            if (AutoMapper.IsStructType(srcType))  {
                ilGenerator.Emit(OpCodes.Ldarg_1, 1); //Save the object with his real type.
                ilGenerator.Emit(OpCodes.Unbox_Any, srcType);
                ilGenerator.DeclareLocal(typeof(Pointer));
                ilGenerator.Emit(OpCodes.Stloc_0);
                ilGenerator.Emit(OpCodes.Ldloca, 4);
                ilGenerator.Emit(OpCodes.St);

            } else
            {
                ilGenerator.DeclareLocal(srcType);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Castclass, srcType); //Cast the source object to his real type.
                ilGenerator.Emit(OpCodes.Stloc_0);//Save the object with his real type.
            }

            if (AutoMapper.IsStructType(destType))  {
                ilGenerator.DeclareLocal(typeof(Pointer));
                ilGenerator.Emit(OpCodes.Unbox_Any, destType);
                ilGenerator.Emit(OpCodes.Ldarga, 2); //Save the object with his real type.
                ilGenerator.Emit(OpCodes.Stloc_1); //Save the object with his real type.
            } else{
                ilGenerator.DeclareLocal(destType);
                ilGenerator.Emit(OpCodes.Ldarg_2);
                ilGenerator.Emit(OpCodes.Castclass, destType); //Cast the source object to his real type.
                ilGenerator.Emit(OpCodes.Stloc_1);//Save the object with his real type.
            }
            
            /* For each source property map it's corresponding property in destination. */
            for (int i = 0; i < srcProperties.Length; i++) {
                origin = srcProperties[i];
                if (attr != null && !origin.IsDefined(attr)) continue;

                /* Gets the corresponding destiny property. */
                if (!dict.TryGetValue(origin.Name, out currentName))
                    currentName = origin.Name;
                destiny = destType.GetProperty(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (destiny == null || (attr != null && !destiny.IsDefined(attr))) continue;

                currentDestType = destiny.PropertyType;
                currentSrcType = origin.PropertyType;

                /* If source and destination type are equal then set destination with source value
                  * else asks for a new mapper and tries to set its value */
                if (currentDestType.Equals(currentSrcType)) {
                    ilGenerator.Emit(OpCodes.Ldloc_1); //Get destination object into stack
                    ilGenerator.Emit(OpCodes.Ldloc_0); //Get source object into stack
                    ilGenerator.Emit(OpCodes.Callvirt, origin.GetGetMethod()); //Get the value of the current source property
                    ilGenerator.Emit(OpCodes.Callvirt, destiny.GetSetMethod()); //Affect the destination property with the value.
                } else {
                    ilGenerator.Emit(OpCodes.Ldtoken, currentSrcType); //Load the source type into evaluation stack
                    ilGenerator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                    ilGenerator.Emit(OpCodes.Ldtoken, currentDestType); //Load the destination type into evaluation stack
                    ilGenerator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                    ilGenerator.Emit(OpCodes.Call, typeof(AutoMapper).GetMethod("Build")); // Call the method build with the 2 parameters from autommaper 
                    //IMapper aux = AutoMapperEmitter.Build(currentSrcType, currentDestType);

                    Type mapperType = typeof(IMapper);
                    ilGenerator.Emit(OpCodes.Dup);
                    ilGenerator.DeclareLocal(mapperType);
                    ilGenerator.Emit(OpCodes.Stloc_2);//Save the mapper into local stack.
                    Label failed3 = ilGenerator.DefineLabel();
                    ilGenerator.Emit(OpCodes.Brfalse, failed3); //Verify if aux == null, if so jump to label

                    ilGenerator.Emit(OpCodes.Ldloc_2);//Get the mapper into eval stack
                    ilGenerator.Emit(OpCodes.Call, typeof(Mapping).GetProperty("Fields").GetGetMethod()); // Get Mapping.Properties
                    //ilGenerator.Emit(OpCodes.Ldfld, typeof(Mapping).GetField("Properties"));
                    ilGenerator.Emit(OpCodes.Callvirt, mapperType.GetMethod("Bind")); //Call the method Bind with Mapping.Properties as parameter 
                    ilGenerator.Emit(OpCodes.Dup);

                    ilGenerator.Emit(OpCodes.Ldloc_0); //Get source object into stack
                    ilGenerator.Emit(OpCodes.Callvirt, origin.GetGetMethod()); //Get the value of the current source property
                    ilGenerator.Emit(OpCodes.Callvirt, mapperType.GetMethod("Map", new Type[] { typeof(object) })); //Call aux.Map(Value)

                    ilGenerator.DeclareLocal(typeof(object));
                    ilGenerator.Emit(OpCodes.Stloc_3); // Save object returned by previous aux.Bind(Mapping.Properties).Map(Value call;

                    ilGenerator.Emit(OpCodes.Call, typeof(Mapping).GetProperty("Properties").GetGetMethod()); // Get Mapping.Properties
                    //ilGenerator.Emit(OpCodes.Ldfld, typeof(Mapping).GetField("Properties"));
                    ilGenerator.Emit(OpCodes.Callvirt, mapperType.GetMethod("Bind")); //Call the method Bind with Mapping.Properties as parameter 

                    ilGenerator.Emit(OpCodes.Ldloc_0); //Get source object into stack
                    ilGenerator.Emit(OpCodes.Callvirt, origin.GetGetMethod()); //Get the value of the current source property
                    ilGenerator.Emit(OpCodes.Callvirt, mapperType.GetMethod("Map", new Type[] { typeof(object) })); //Call aux.Map(Value)
                    ilGenerator.Emit(OpCodes.Ldloc_3);
                    ilGenerator.Emit(OpCodes.Call, typeof(Emitter).GetMethod("joinData"));
                    //ilGenerator.Emit(OpCodes.Castclass, currentDestType);
                    ilGenerator.Emit(OpCodes.Stloc_3);
                    ilGenerator.Emit(OpCodes.Ldloc_1);//Get the dest object into eval stack
                    ilGenerator.Emit(OpCodes.Ldloc_3);//Get the object into eval stack
                    ilGenerator.Emit(OpCodes.Castclass, currentDestType);
                    ilGenerator.Emit(OpCodes.Callvirt, destiny.GetSetMethod()); //Affect the destination property with the value.
                    ilGenerator.MarkLabel(failed3);
                    /*if (aux != null) { 
                     * 1 = aux.Bind(Mapping.Properties).Map(value)
                     * 2 = aux.Bind(Mapping.Fields).Map(value)
                     * tomap = joinData(1, 2)
                       destiny.SetValue(destObject, );
                    }*/
                }
            }
            ilGenerator.Emit(OpCodes.Ret);
            emittedClass = typeBuilder.CreateType();
            ab.Save("MappingAssembly.dll");
            addToCache(srcType, destType, attr, emittedClass);
            return emittedClass;
        }
    }
}

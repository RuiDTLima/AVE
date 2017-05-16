using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

namespace MapperEmit.Emiter
{
    public class PropertiesEmitter : Emitter {
        /* Contains the already emitted classes that maps the first type into second type. */
        private Dictionary<KeyValuePair<Type, Type>, Type> emittedClasses = new Dictionary<KeyValuePair<Type, Type>, Type>();

        public override Type EmitClass(Type srcType, Type destType, ModuleBuilder moduleBuilder, Dictionary<string, string> dict)  {
            TypeBuilder typeBuilder = moduleBuilder.DefineType("PropertyMapping" + srcType.Name + "To" + destType.GetType().Name,
                                                               TypeAttributes.Public);
            /* Define that the emittied class is a Mapping */
            typeBuilder.AddInterfaceImplementation(typeof(MappingEmit));
            /* Arrange the method that would look like public void Map(object srcObject, object destObject Type attribute, Dictionary<string, string> dic) */
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Map", MethodAttributes.Public,
                                                                   typeof(void), new Type[] { typeof(object), typeof(object)});

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();

            PropertyInfo[] srcProperties = srcType.GetProperties();

            PropertyInfo destiny, origin;
            string currentName;
            Type currentDestType, currentSrcType;

            //Get the source object and dest object to cast from object to their types. 
            /* public void Map(object src, object dest) */
            ilGenerator.Emit(OpCodes.Ldarg_1); //Get the source object into stack
            ilGenerator.Emit(OpCodes.Castclass, srcType); //Cast the source object to his real type.
            ilGenerator.Emit(OpCodes.Stloc_0);//Save the object with his real type.

            ilGenerator.Emit(OpCodes.Ldarg_2); //Get the dest object into stack
            ilGenerator.Emit(OpCodes.Castclass, destType); //Cast the dest object to his real type.
            ilGenerator.Emit(OpCodes.Stloc_1);//Save the object with his real type.

            /* For each source property map it's corresponding property in destination. */
            for (int i = 0; i < srcProperties.Length; i++) {
                origin = srcProperties[i];
                /* Gets the corresponding destiny property. */
                if (!dict.TryGetValue(origin.Name, out currentName))
                    currentName = origin.Name;
                destiny = destType.GetProperty(currentName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (destiny == null) continue;

                currentDestType = destiny.PropertyType;
                currentSrcType = origin.PropertyType;

                /* If source and destination type are equal then set destination with source value
                  * else asks for a new mapper and tries to set its value */
                if (currentDestType.Equals(currentSrcType)) {
                    ilGenerator.Emit(OpCodes.Ldloc_1);//Get destination object into stack
                    ilGenerator.Emit(OpCodes.Ldloc_0);//Get source object into stack
                    ilGenerator.Emit(OpCodes.Callvirt, origin.GetGetMethod());//Get the value of the current source property
                    ilGenerator.Emit(OpCodes.Callvirt, destiny.GetSetMethod());//Affect the destination property with the value.
                } else {
                    IMapper aux = AutoMapperEmitter.Build(currentSrcType, currentDestType);
                    if (aux != null) { 
                        aux.Bind(Emitter.Properties);
                        aux.Map(Value);
                        aux.Bind(Emitter.Fields);
                        aux.Map(Value);
                        /* Asks for the classes to map the fields and properties. */
                        destiny.SetValue(destObject, Value);
                    }
                }
            }
        }
    }
}

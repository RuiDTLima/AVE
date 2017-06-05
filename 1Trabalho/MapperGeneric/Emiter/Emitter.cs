using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperGeneric {
    public abstract class Emitter {
        public abstract MappingEmit EmitClass(Type srcType, Type destType, Type attr, Dictionary<string, string> dict, Dictionary<string, object> dictResult);

        public abstract Type EmitClass(Type destType);

        /* Contains the already emitted classes that maps the first type into second type. */
        protected Dictionary<KeyValuePair<Type, Type>, MappingEmit> emittedClasses = new Dictionary<KeyValuePair<Type, Type>, MappingEmit>();

        /* Contains the already emitted classes that maps the first type into second type with custom attribute. */
        protected Dictionary<KeyValuePair<Type, Type>, MappingEmit> emittedClassesAttribute = new Dictionary<KeyValuePair<Type, Type>, MappingEmit>();

        protected Dictionary<Type, ConstructorEmit> emittedConstructors = new Dictionary<Type, ConstructorEmit>();

        protected void addToCache(Type type, ConstructorEmit emittedConstructor) {
            emittedConstructors.Add(type, emittedConstructor);
        }

        /* Add the emitted class into cache. */
        protected void addToCache(Type srcType, Type destType, Type attr, MappingEmit emittedClass) {
            KeyValuePair<Type, Type> key = new KeyValuePair<Type, Type>(srcType, destType);
            if (attr == null)
                emittedClasses.Add(key, emittedClass);
            emittedClassesAttribute.Add(key, emittedClass);
        }

        protected bool IsInCache(Type Type, out ConstructorEmit emittedConstructor) {
            return emittedConstructors.TryGetValue(Type, out emittedConstructor);
        }

        /* Verify if for those types already exist a emitted class and if so affects it.  */
        protected bool IsInCache(Type srcType, Type destType, Type attr, out MappingEmit emittedClass) {
            KeyValuePair<Type, Type> key = new KeyValuePair<Type, Type>(srcType, destType);
            if (attr == null)
                return emittedClasses.TryGetValue(key, out emittedClass);
            return emittedClassesAttribute.TryGetValue(key, out emittedClass);
        }

        /* Receive two objects, one only has properties with values and the other only has fields with values
         * combines the information of each object into one and returns it. 
         */
        public static object joinData(object objProperties, object objFields) {
            if ((objProperties == null || objProperties == null) || objProperties.GetType() != objFields.GetType()) return null;

            PropertyInfo[] properties = objFields.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++) {
                PropertyInfo aux = objProperties.GetType().GetProperty(properties[i].Name);
                properties[i].SetValue(objFields, aux.GetValue(objProperties));
            }
            return objFields;
        }
    }
}
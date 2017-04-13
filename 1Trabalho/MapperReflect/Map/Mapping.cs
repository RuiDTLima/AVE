using System;
using System.Collections.Generic;
using System.Reflection;

namespace MapperReflect
{
    public class Mapping
    {
        public static MappingFields Fields = new MappingFields();
        public static MappingProperties Properties = new MappingProperties();
        public static MappingToMap ToMap = new MappingToMap();
        protected Type Attribute { get; set; }

        protected Dictionary<KeyValuePair<Type, Type>, IMapper> cacheContainer = Cache.cache;

        public Mapping(Type Attribute) {
            this.Attribute = Attribute;
        }

        public Mapping(){}
        

        public virtual object Map(object srcObject, Type src, Type dest, Dictionary<String, String> dict) {
            if (Attribute == null) return Properties.Map(srcObject, src, dest, dict);
            return ToMap.Map(srcObject, src, dest, Attribute, dict);  
        }

        public object[] Map(object[] srcObject, Type src, Type dest, Dictionary<String, String> dict) {
            object[] obj = (object[])Array.CreateInstance(dest, srcObject.Length);
            for (int i = 0; i < srcObject.Length; ++i)
            {
                obj[i] = Map(srcObject[i], src, dest, dict);
            }
            return obj;
        }

    }
}

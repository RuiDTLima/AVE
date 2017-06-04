using System;

namespace MapperGeneric {
    public class Teacher {
        [ToMap]
        public string Name { get; set; }

        public string _Name;

        [ToMap]
        public int Id { get; set; }

        public int _Id;

        public Organization Org { get; set; }

        public Organization _Org;
    }
}

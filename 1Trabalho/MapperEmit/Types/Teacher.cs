using System;

namespace MapperEmit {
    public class Teacher {
        [ToMap]
        public string Name { get; set; }

        public String _Name;

        [ToMap]
        public int Id { get; set; }

        public int _Id;

        public School Org { get; set; }

        public School _Org;
    }
}

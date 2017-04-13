using System;

namespace MapperReflect
{
    public class Person
    {
        [ToMap]
        public string Name { get; set; }
        [ToMap]
        public int Id { get; set; }

        public Subject[] Subjects { get; set; }

        private Organization Org;
        
    }
}

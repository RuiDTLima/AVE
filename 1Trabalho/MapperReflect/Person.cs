using System;

namespace MapperReflect
{
    public class Person
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public Subject[] Subjects { get; set; }

        private Organization Org;
    }
}

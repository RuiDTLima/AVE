using System;

namespace MapperReflect
{
    public class Student
    {
        public Course[] Courses { get; set; }

        [ToMap]
        public string Name { get; set; }
        [ToMap]
        public int Nr { get; set; }

        private School Org;
    }
}

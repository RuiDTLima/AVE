using System;

namespace MapperReflect
{
    public class Student
    {
        public Course[] Courses { get; set; }

        public string Name { get; set; }

        public int Nr { get; set; }

        private School Org;
    }
}

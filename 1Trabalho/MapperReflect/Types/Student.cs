using System;

namespace MapperReflect
{
    public class Student
    {
        public Course[] Courses { get; set; }

        [ToMapAttribute]
        public string Name { get; set; }
        [ToMapAttribute]
        public string _Name;
        [ToMapAttribute]
        public int Nr { get; set; }
        [ToMapAttribute]
        public int _Nr;

        public School Org { get; set; }
        public School _Org;
    }
}

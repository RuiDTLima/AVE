using System;


namespace MapperReflect
{
    class App
    {
        static void Main(string[] args)
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person));
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = (Person)m.Map(s);
            Assert.Equal(s.Name, p.Name);
        }
    }
}

using System;

namespace MapperReflect
{
    public class App
    {
        static void Main(string[] args)
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student),typeof(Person)).Bind(new MappingProperties()).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = (Person)m.Map(s);
            //Assert.Equal(s.Name, p.Name);
        }
    }
}

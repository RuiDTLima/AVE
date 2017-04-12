using System;

namespace MapperReflect
{
    public class App
    {
        static void Main(string[] args)
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Subject)).Bind(new MappingProperties()).Match("Nr", "Id");
            Teacher s = new Teacher { Id = 27721, Name = "Ze Manel" };
            Subject p = (Subject)m.Map(s);
        }
    }
}

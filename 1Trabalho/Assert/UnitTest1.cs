using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperReflect;

namespace Trabalho1Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public static void M() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(new MappingProperties()).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = (Person)m.Map(s);
            Assert.Equals(s.Name, p.Name);
        }
    }
}

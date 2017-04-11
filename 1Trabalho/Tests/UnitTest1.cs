using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperReflect;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEqualTypesDifferentNames()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(new MappingProperties()).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
        }

        [TestMethod]
        public void TestSameTypeSameName()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Person));
            Teacher s = new Teacher { Id = 27721, Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
        }
    }
}

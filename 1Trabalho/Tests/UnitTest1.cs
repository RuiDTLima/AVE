using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperReflect;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPropertiesEqualTypesDifferentNames()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(Mapping.Properties).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Id);
        }

        [TestMethod]
        public void TestPropertiesSameTypeSameName()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Person)).Bind(Mapping.Properties);
            Teacher s = new Teacher { Id = 27721, Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Id, p.Id);
        }

        public struct Test{
            public string Name { get; set; }

            public int Id { get; set; }
        }

        [TestMethod]
        public void TestPropertiesWithValueType()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Test), typeof(Person)).Bind(Mapping.Properties);
            Test s = new Test { Id = 27721, Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Id, p.Id);
        }

        [TestMethod]
        public void TestPropertiesArray()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Teacher)).Bind(Mapping.Properties).Match("Nr", "Id");
            Student[] stds = { new Student { Nr = 27721, Name = "António" }, new Student { Nr = 11111, Name = "Joana" } };
            Teacher[] ps = (Teacher[])m.Map(stds); // Erro ao converter

            Assert.AreEqual(stds[0].Name, ps[0].Name);
            Assert.AreEqual(stds[0].Nr, ps[0].Id);

            Assert.AreEqual(stds[1].Name, ps[1].Name);
            Assert.AreEqual(stds[1].Nr, ps[1].Id);
        }


        
        [TestMethod]
        public void TestToMapAttribute() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(new Mapping(typeof(ToMapAttribute))).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "António" };
            Person p = (Person)m.Map(s); 

            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Id);
        }
        

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperReflect;

namespace Tests
{
    [TestClass]
    public class UnitTest1 { 
    
        /************************************** PropertiesTests **********************************************/
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
        public void TestPropertiesWithDifferentReferences()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Teacher)).Bind(Mapping.Properties).Match("Nr", "Id"); ;
            Student s = new Student { Nr = 27721, Name = "António", Org = new School {Name = "ISEL"} };
            Teacher t = (Teacher)m.Map(s);
            Assert.AreEqual(s.Name, t.Name);
            Assert.AreEqual(s.Nr, t.Id);
            Assert.AreEqual(s.Org.MembersIds, t.Org.MembersIds);
            Assert.AreEqual(s.Org.Name, t.Org.Name);
        }

        [TestMethod]
        public void TestPropertiesArray()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Teacher)).Bind(Mapping.Properties).Match("Nr", "Id");
            Student[] stds = { new Student { Nr = 27721, Name = "António" }, new Student { Nr = 11111, Name = "Joana" } };
            Teacher[] ps = (Teacher[])m.Map(stds); 

            Assert.AreEqual(stds[0].Name, ps[0].Name);
            Assert.AreEqual(stds[0].Nr, ps[0].Id);

            Assert.AreEqual(stds[1].Name, ps[1].Name);
            Assert.AreEqual(stds[1].Nr, ps[1].Id);
        }

        /************************************** FieldsTests **************************************************/
        [TestMethod]
        public void TestFieldsEqualTypesDifferentNames()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Teacher)).Bind(Mapping.Fields).Match("_Nr", "_Id");
            Student s = new Student { _Nr = 27721, _Name = "Ze Manel" };
            Teacher p = (Teacher)m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Nr, p._Id);
        }

        [TestMethod]
        public void TestFieldsSameTypeSameName()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Person)).Bind(Mapping.Fields);
            Teacher s = new Teacher { _Id = 27721, _Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Id, p._Id);
        }

        [TestMethod]
        public void TestFieldsWithDifferentReferences() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(Mapping.Fields).Match("_Nr", "_Id"); ;
            Student s = new Student { _Nr = 27721, _Name = "António", _Org = new School { Name = "ISEL", MembersIds = new int[]{ 1,2 } } };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Nr, p._Id);
            Assert.AreEqual(s._Org.MembersIds[0], p._Org.MembersIds[0]);
            Assert.AreEqual(s._Org.MembersIds[1], p._Org.MembersIds[1]);
            Assert.AreEqual(s._Org.Name, p._Org.Name);
        }


        /************************************** CostumAttributeTests *****************************************/
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

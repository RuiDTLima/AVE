﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperEmit;

namespace Tests
{
    [TestClass]
    public class UnitTestEmit
    {
        public class class1 {
            void map(object src, object dest) {
                Test t = (Test) src;
                Person p = (Person) dest;
                p.Name = t.Name;
                p.Id = t.Id;
            }
        }
        public struct Test
        {
            public string Name { get; set; }

            [ToMap]
            public string _Name;

            public int Id { get; set; }

            [ToMap]
            public int _Id;
        }

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
            Student s = new Student { Nr = 27721, Name = "António", Org = new School { Name = "ISEL" } };
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

        [TestMethod]
        public void TestFieldsSameTypeSameName()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Person)).Bind(Mapping.Fields);
            Teacher s = new Teacher { _Id = 27721, _Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Id, p._Id);
        }
    }
}

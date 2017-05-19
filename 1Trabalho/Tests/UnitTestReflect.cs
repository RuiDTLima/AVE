using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperEmit;

namespace Tests
{
    [TestClass]
    public class UnitTestReflect {

        public struct Test
        {
            public string Name { get; set; }

            [ToMap]
            public string _Name;

            public int Id { get; set; }

            [ToMap]
            public int _Id;
        }
        /************************************** GeneralTests *************************************************/
        [TestMethod]
        public void TestNotPossibleType() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(int), typeof(Person));
            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void TestCache() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Person));
            Mapper mCache = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Person));
            Assert.AreSame(m, mCache);
        }

        /************************************** PropertiesTests **********************************************/
        [TestMethod]
        public void TestPropertiesEqualTypesDifferentNames()  {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(Mapping.Properties).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Id);
        }

        [TestMethod]
        public void TestPropertiesSameTypeSameName() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Person)).Bind(Mapping.Properties);
            Teacher s = new Teacher { Id = 27721, Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Id, p.Id);
        }



        [TestMethod]
        public void TestPropertiesWithValueType() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Test), typeof(Person)).Bind(Mapping.Properties);
            Test s = new Test { Id = 27721, Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Id, p.Id);
        }

        [TestMethod]
        public void TestPropertiesWithDifferentReferences() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Teacher)).Bind(Mapping.Properties).Match("Nr", "Id"); ;
            Student s = new Student { Nr = 27721, Name = "António", Org = new School {Name = "ISEL"} };
            Teacher t = (Teacher)m.Map(s);
            Assert.AreEqual(s.Name, t.Name);
            Assert.AreEqual(s.Nr, t.Id);
            Assert.AreEqual(s.Org.MembersIds, t.Org.MembersIds);
            Assert.AreEqual(s.Org.Name, t.Org.Name);
        }

        [TestMethod]
        public void TestPropertiesArray() {
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
        public void TestFieldsEqualTypesDifferentNames() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Teacher)).Bind(Mapping.Fields).Match("_Nr", "_Id");
            Student s = new Student { _Nr = 27721, _Name = "Ze Manel" };
            Teacher p = (Teacher)m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Nr, p._Id);
        }

        [TestMethod]
        public void TestFieldsSameTypeSameName() {
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

        [TestMethod]
        public void TestFieldsArray() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Teacher)).Bind(Mapping.Fields).Match("Nr", "Id");
            Student[] stds = { new Student { _Nr = 27721, _Name = "António" }, new Student { _Nr = 11111, _Name = "Joana" } };
            Teacher[] ps = (Teacher[])m.Map(stds);

            Assert.AreEqual(stds[0]._Name, ps[0]._Name);
            Assert.AreEqual(stds[0]._Nr, ps[0]._Id);

            Assert.AreEqual(stds[1]._Name, ps[1]._Name);
            Assert.AreEqual(stds[1]._Nr, ps[1]._Id);
        }

        [TestMethod]
        public void TestFieldsWithValueType() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Test), typeof(Person)).Bind(Mapping.Fields);
            Test s = new Test { _Id = 27721, _Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Id, p._Id);
        }


        /************************************** CostumAttributeTests *****************************************/
        [TestMethod]
        public void TestToMapAttribute()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(new Mapping(typeof(ToMapAttribute))).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "António" };
            Person p = (Person)m.Map(s);

            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Id);
        }

        [TestMethod]
        public void TestToMapAttributeArray()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(new Mapping(typeof(ToMapAttribute))).Match("_Nr", "_Id");
            Student[] stds = { new Student { _Nr = 27721, _Name = "António" }, new Student { _Nr = 11111, _Name = "Joana" } };
            Person[] ps = (Person[])m.Map(stds);

            Assert.AreEqual(stds[0]._Name, ps[0]._Name);
            Assert.AreEqual(stds[0]._Nr, ps[0]._Id);

            Assert.AreEqual(stds[1]._Name, ps[1]._Name);
            Assert.AreEqual(stds[1]._Nr, ps[1]._Id);
        }

        [TestMethod]
        public void TestToMapAttributeWithValueType()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Test), typeof(Person)).Bind(new Mapping(typeof(ToMapAttribute)));
            Test s = new Test { _Id = 27721, _Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Id, p._Id);
        }

        [TestMethod]
        public void TestToMapAttributeWithDifferentReferences()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(new Mapping(typeof(ToMapAttribute))).Match("_Nr", "_Id"); ;
            Student s = new Student { _Nr = 27721, _Name = "António", _Org = new School { Name = "ISEL", MembersIds = new int[] { 1, 2 } } };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Nr, p._Id);
            Assert.AreEqual(s._Org.MembersIds[0], p._Org.MembersIds[0]);
            Assert.AreEqual(s._Org.MembersIds[1], p._Org.MembersIds[1]);
            Assert.AreEqual(s._Org.Name, p._Org.Name);
        }

        [TestMethod]
        public void TestToMapAttributeSameTypeSameName()
        {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Teacher), typeof(Person)).Bind(new Mapping(typeof(ToMapAttribute)));
            Teacher t = new Teacher { Id = 27721, Name = "António" };
            Person p = (Person)m.Map(t);

            Assert.AreEqual(t.Name, p.Name);
            Assert.AreEqual(t.Id, p.Id);
        }


    }
}

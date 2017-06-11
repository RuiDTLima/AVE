using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperGeneric;
using System.Collections.Generic;

namespace TestGeneric
{
    [TestClass]
    public class UnitTestGeneric
    {
        public struct Test
        {
            public string Name { get; set; }

            [ToMap]
            public string _Name;

            public int Id { get; set; }

            [ToMap]
            public int _Id;
        }
        /************************************** InumerableTest **********************************************/
        [TestMethod]
        public void InumerableTest()
        {
            List<Student> list = new List<Student>();
            list.Add(new Student { Nr = 11111, Name = "Catarina" });
            list.Add(new Student { Nr = 22222, Name = "Antonio" });
            list.Add(new Student { Nr = 33333, Name = "Rogerio" });

            IMapperGeneric<Student, Teacher> m = AutoMapper.Build<Student, Teacher>().Bind(Mapping.Properties).Match("Nr", "Id");
            IEnumerable<Teacher> enumerable = m.MapLazy(list);
            IEnumerator<Teacher> enumerator = enumerable.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(enumerator.Current.Id, 11111);
            Assert.AreEqual(enumerator.Current.Name, "Catarina");
            Assert.IsTrue(enumerator.MoveNext());

            Assert.AreEqual(enumerator.Current.Id, 22222);
            Assert.AreEqual(enumerator.Current.Name, "Antonio");
            Assert.IsTrue(enumerator.MoveNext());

            Assert.AreEqual(enumerator.Current.Id, 33333);
            Assert.AreEqual(enumerator.Current.Name, "Rogerio");
           
            Assert.IsFalse(enumerator.MoveNext());


        }
        /************************************** TestFor **********************************************/
        [TestMethod]
        public void TestForWithReferenceType()
        {
            IMapperGeneric<Person, Student> m = AutoMapper.Build<Person, Student>()
                                                          .Bind(Mapping.Fields)
                                                          .Match("_Id", "_Nr")
                                                          .For("_Name", () => "Antonio");
            Person p = new Person { _Id = 27251, _Name = "Rogerio" };
            Student s = m.Map(p);
            Assert.AreEqual(s._Name, "Antonio");
            Assert.AreEqual(s._Nr, p._Id);
        }

        /************************************** PropertiesTests **********************************************/

        [TestMethod]
        public void TestPropertiesEqualTypesDifferentNames()
        {
            IMapperGeneric<Student, Person> m = AutoMapper.Build<Student, Person>().Bind(Mapping.Properties).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Id);
        }

        [TestMethod]
        public void TestPropertiesSameTypeSameName()
        {
            IMapperGeneric<Person, Teacher> m = AutoMapper.Build<Person, Teacher>().Bind(Mapping.Properties);
            Person p = new Person { Id = 27721, Name = "António" };
            Teacher t = m.Map(p);
            Assert.AreEqual(p.Name, t.Name);
            Assert.AreEqual(p.Id, t.Id);
        }


        [TestMethod]
        public void TestPropertiesWithDifferentReferences()
        {
            IMapperGeneric<Student, Teacher> m = AutoMapper.Build<Student, Teacher>().Bind(Mapping.Properties).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "António", Org = new School { Name = "ISEL" } };
            Teacher t = m.Map(s);
            Assert.AreEqual(s.Name, t.Name);
            Assert.AreEqual(s.Nr, t.Id);
            Assert.AreEqual(s.Org.MembersIds, t.Org.MembersIds);
            Assert.AreEqual(s.Org.Name, t.Org.Name);
        }


        [TestMethod]
        public void TestPropertiesWithValueType()
        {
            IMapperGeneric<Test, Person> m = AutoMapper.Build<Test, Person>().Bind(Mapping.Properties);
            Test s = new Test { Id = 27721, Name = "António" };
            Person p = m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Id, p.Id);
        }


        [TestMethod]
        public void TestPropertiesArray()
        {
            IMapperGeneric<Student, Teacher> m = AutoMapper.Build<Student, Teacher>().Bind(Mapping.Properties).Match("Nr", "Id");
            Student[] stds = { new Student { Nr = 27721, Name = "António" }, new Student { Nr = 11111, Name = "Joana" } };
            List<Teacher> ps = m.Map<List<Teacher>>(stds);

            Assert.AreEqual(stds[0].Name, ps[0].Name);
            Assert.AreEqual(stds[0].Nr, ps[0].Id);

            Assert.AreEqual(stds[1].Name, ps[1].Name);
            Assert.AreEqual(stds[1].Nr, ps[1].Id);
        }

        /************************************** FieldsTests **************************************************/
        [TestMethod]
        public void TestFieldsEqualTypesDifferentNames()
        {
            IMapperGeneric<Student, Teacher> m = AutoMapper.Build<Student, Teacher>().Bind(Mapping.Fields).Match("_Nr", "_Id");
            Student s = new Student { _Nr = 27721, _Name = "Ze Manel" };
            Teacher p = m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Nr, p._Id);
        }

        [TestMethod]
        public void TestFieldsSameTypeSameName()
        {
            IMapperGeneric<Teacher, Person> m = AutoMapper.Build<Teacher, Person>().Bind(Mapping.Fields);
            Teacher s = new Teacher { _Id = 27721, _Name = "António" };
            Person p = m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Id, p._Id);
        }

        [TestMethod]
        public void TestFieldsWithDifferentReferences()
        {
            IMapperGeneric<Student, Person> m =  AutoMapper.Build<Student, Person>().Bind(Mapping.Fields).Match("_Nr", "_Id"); ;
            Student s = new Student { _Nr = 27721, _Name = "António", _Org = new School { Name = "ISEL", MembersIds = new int[] { 1, 2 } } };
            Person p = m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Nr, p._Id);
            Assert.AreEqual(s._Org.MembersIds[0], p._Org.MembersIds[0]);
            Assert.AreEqual(s._Org.MembersIds[1], p._Org.MembersIds[1]);
            Assert.AreEqual(s._Org.Name, p._Org.Name);
        }

        [TestMethod]
        public void TestFieldsArray()
        {
            IMapperGeneric<Student, Teacher> m =  AutoMapper.Build<Student, Teacher>().Bind(Mapping.Fields).Match("_Nr", "_Id");
            Student[] stds = { new Student { _Nr = 27721, _Name = "António" }, new Student { _Nr = 11111, _Name = "Joana" } };
            List<Teacher> ps = m.Map<List<Teacher>>(stds);

            Assert.AreEqual(stds[0]._Name, ps[0]._Name);
            Assert.AreEqual(stds[0]._Nr, ps[0]._Id);

            Assert.AreEqual(stds[1]._Name, ps[1]._Name);
            Assert.AreEqual(stds[1]._Nr, ps[1]._Id);
        }

        [TestMethod]
        public void TestFieldsWithValueType()
        {
            IMapperGeneric<Test, Person> m = AutoMapper.Build<Test, Person>().Bind(Mapping.Fields);
            Test s = new Test { _Id = 27721, _Name = "António" };
            Person p = m.Map(s);
            Assert.AreEqual(s._Name, p._Name);
            Assert.AreEqual(s._Id, p._Id);
        }

    }
}

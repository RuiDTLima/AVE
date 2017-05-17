using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperEmit;

namespace Tests
{
    [TestClass]
    public class UnitTestEmit
    {
        [TestMethod]
        public void TestPropertiesSameTypeSameName() {
            Mapper m = (Mapper)AutoMapperEmitter.Build(typeof(Teacher), typeof(Person)).Bind(Mapping.Properties);
            Teacher s = new Teacher { Id = 27721, Name = "António" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Id, p.Id);
        }

        [TestMethod]
        public void TestPropertiesWithDifferentReferences()
        {
            Mapper m = (Mapper)AutoMapperEmitter.Build(typeof(Student), typeof(Teacher)).Bind(Mapping.Properties).Match("Nr", "Id"); ;
            Student s = new Student { Nr = 27721, Name = "António", Org = new School { Name = "ISEL" } };
            Teacher t = (Teacher)m.Map(s);
            Assert.AreEqual(s.Name, t.Name);
            Assert.AreEqual(s.Nr, t.Id);
            Assert.AreEqual(s.Org.MembersIds, t.Org.MembersIds);
            Assert.AreEqual(s.Org.Name, t.Org.Name);
        }
    }
}

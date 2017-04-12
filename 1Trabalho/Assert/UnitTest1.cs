﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperReflect;

namespace MapperTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void M() {
            Mapper m = (Mapper)AutoMapper.Build(typeof(Student), typeof(Person)).Bind(new MappingProperties()).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
        }
    }
}

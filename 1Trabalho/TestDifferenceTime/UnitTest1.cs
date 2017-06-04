using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;


namespace TestDifferenceTime
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDifferenceTime()
        {

            Stopwatch watch = new Stopwatch();

            for (int i = 0; i < 10; i++)
            {
                ExecuteReflect();
            }

            for (int i = 0; i < 10; i++)
            {
                ExecuteEmit();
            }

            watch.Start();
            ExecuteReflect();
            watch.Stop();
            long reflection = watch.Elapsed.Ticks;

            watch.Restart();
            ExecuteEmit();
            watch.Stop();
            long emit = watch.Elapsed.Ticks;

            Assert.IsTrue(emit < reflection);
        }

        private void ExecuteEmit()
        {
            MapperGeneric.Mapper m = (MapperGeneric.Mapper)MapperGeneric.AutoMapper.Build(typeof(MapperGeneric.Student), typeof(MapperGeneric.Teacher)).Bind(MapperGeneric.Mapping.Properties).Match("Nr", "Id");
            MapperGeneric.Student[] stds = { new MapperGeneric.Student { Nr = 27721, Name = "António" }, new MapperGeneric.Student { Nr = 11111, Name = "Joana" } };
            MapperGeneric.Teacher[] ps = (MapperGeneric.Teacher[])m.Map(stds);
        }

        private void ExecuteReflect()
        {
            MapperReflect.Mapper m = (MapperReflect.Mapper)MapperReflect.AutoMapper.Build(typeof(MapperReflect.Student), typeof(MapperReflect.Teacher)).Bind(MapperReflect.Mapping.Properties).Match("Nr", "Id");
            MapperReflect.Student[] stds = { new MapperReflect.Student { Nr = 27721, Name = "António" }, new MapperReflect.Student { Nr = 11111, Name = "Joana" } };
            MapperReflect.Teacher[] ps = (MapperReflect.Teacher[])m.Map(stds);
        }
    }
}

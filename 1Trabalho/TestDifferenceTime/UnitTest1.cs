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
            MapperEmit.Mapper m = (MapperEmit.Mapper)MapperEmit.AutoMapper.Build(typeof(MapperEmit.Student), typeof(MapperEmit.Teacher)).Bind(MapperEmit.Mapping.Properties).Match("Nr", "Id");
            MapperEmit.Student[] stds = { new MapperEmit.Student { Nr = 27721, Name = "António" }, new MapperEmit.Student { Nr = 11111, Name = "Joana" } };
            MapperEmit.Teacher[] ps = (MapperEmit.Teacher[])m.Map(stds);
        }

        private void ExecuteReflect()
        {
            MapperReflect.Mapper m = (MapperReflect.Mapper)MapperReflect.AutoMapper.Build(typeof(MapperReflect.Student), typeof(MapperReflect.Teacher)).Bind(MapperReflect.Mapping.Properties).Match("Nr", "Id");
            MapperReflect.Student[] stds = { new MapperReflect.Student { Nr = 27721, Name = "António" }, new MapperReflect.Student { Nr = 11111, Name = "Joana" } };
            MapperReflect.Teacher[] ps = (MapperReflect.Teacher[])m.Map(stds);
        }
    }
}

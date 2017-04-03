using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperReflect
{
    class Person
    {
        [Mapping.Properties]
        public string Name { get; set; }

        [Mapping.Properties]
        public int Id { get; set; }
    }
}

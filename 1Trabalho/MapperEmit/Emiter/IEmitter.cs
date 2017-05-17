using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MapperEmit.Emiter
{
    public interface IEmitter {
        Type EmitClass(Type srcType, Type destType, Type attr, Dictionary<String, String> dict);
    }

}

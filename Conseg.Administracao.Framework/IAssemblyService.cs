using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Conseg.Administracao.Framework
{
    public interface IAssemblyService
    {
        List<Assembly> Assemblies { get; }
        List<Type> Entities { get; }
        List<Type> Models { get; }
        List<Type> Controllers { get; }
        List<Type> ApiControllers { get; }
        List<Type> StartupTasks { get; }
    }
}

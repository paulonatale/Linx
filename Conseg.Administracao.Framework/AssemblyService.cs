using Conseg.Administracao.Domain.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;


namespace Conseg.Administracao.Framework
{
    public class AssemblyService : IAssemblyService
    {
        private List<Assembly> _assemblies = new List<Assembly>();
        private List<Type> _entities = new List<Type>();
        private List<Type> _models = new List<Type>();
        private List<Type> _controllers = new List<Type>();
        private List<Type> _apiControllers = new List<Type>();
        private List<Type> _startupTasks = new List<Type>();

        public List<Assembly> Assemblies
        {
            get { return _assemblies; }
            private set { _assemblies = value; }
        }
        public List<Type> Entities
        {
            get { return _entities; }
            private set { _entities = value; }
        }
        public List<Type> Models
        {
            get { return _models; }
            private set { _models = value; }
        }
        public List<Type> Controllers
        {
            get { return _controllers; }
            private set { _controllers = value; }
        }
        public List<Type> ApiControllers
        {
            get { return _apiControllers; }
            private set { _apiControllers = value; }
        }
        public List<Type> StartupTasks
        {
            get { return _startupTasks; }
            private set { _startupTasks = value; }
        }

        public void LoadAssemblies()
        {
            Assemblies = GetAssemblies().ToList();
            foreach (Assembly assembly in Assemblies)
            {
                Type[] types = assembly.GetTypes();
                Entities.AddRange(types.Where(x => x.BaseType == typeof(BaseEntity)));
                Models.AddRange(types.Where(x => x.BaseType == typeof(BaseModel)));
                Controllers.AddRange(types.Where(x => x.BaseType == typeof(Controller)));
                ApiControllers.AddRange(types.Where(d => d.BaseType == typeof(ApiController)));

            }
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            List<Assembly> list, assemblies;
            assemblies = new List<Assembly>();
            list = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Conseg")).ToList();

            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Conseg")));

            foreach (Assembly asm in list)
            {
                foreach (var referenceName in GetReferencedAssembliesByAssembly(asm.GetName()))
                {
                    if (assemblies.Count(x => x.FullName == referenceName.FullName) == 0)
                    {
                        assemblies.Add(AppDomain.CurrentDomain.Load(referenceName));
                    }
                }
            }

            return assemblies;
        }

        public static IEnumerable<AssemblyName> GetReferencedAssembliesByAssembly(AssemblyName asmName)
        {
            Assembly asm = AppDomain.CurrentDomain.Load(asmName);
            List<AssemblyName> listAssemblyName = new List<AssemblyName>();

            foreach (var assemblyName in asm.GetReferencedAssemblies().Where(x => x.FullName.StartsWith("Conseg")))
            {
                listAssemblyName.AddRange(GetReferencedAssembliesByAssembly(assemblyName));
            }

            return listAssemblyName;
        }
    }
}

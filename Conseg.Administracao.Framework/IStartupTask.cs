using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Conseg.Administracao.Framework
{
    public interface IStartupTask
    {
        /// <summary>
        /// Executes a task
        /// </summary>
        void Execute(IContainer container);

        /// <summary>
        /// Order
        /// </summary>
        int Order { get; }
    }
}

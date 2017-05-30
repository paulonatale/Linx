
using Conseg.Administracao.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Entities
{

    public class Usuario : BaseEntity
    {       
        public string Nome { get; set; }      
        public string Login { get; set; }      
        public string Password { get; set; }
    }
}

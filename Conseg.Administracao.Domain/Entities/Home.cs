using Conseg.Administracao.Domain.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Entities
{
    public class Home : BaseEntity
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Index { get; set; }
        public string Url { get; set; }
    }
}

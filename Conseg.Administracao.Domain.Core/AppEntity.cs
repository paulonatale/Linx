using Conseg.Administracao.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public class AppEntity : BaseEntity
    {
        public string Name { get; set; }
        public string TypeFullname { get; set; }
        public string Namespace { get; set; }
        public string Assembly { get; set; }
        public virtual ICollection<AppEntityField> EntityFields { get; }
    }
}

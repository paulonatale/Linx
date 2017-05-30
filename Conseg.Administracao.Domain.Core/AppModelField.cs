using Conseg.Administracao.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public class AppModelField : BaseEntity
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string DisplayName { get; set; }
    }
}

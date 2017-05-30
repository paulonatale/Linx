using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Conseg.Administracao.Domain.Core.IRepository;
using Conseg.Administracao.Domain.Core;

namespace Conseg.Administracao.Domain.ModelView
{
   public  class HomeModel : BaseModel
    {
      
            public string Controller { get; set; }
            public string Action { get; set; }
            public string Index { get; set; }
            public string Url { get; set; }
        
    }
}

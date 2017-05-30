using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public class BaseModel
    {
        //[UIHint("Id")]
        public virtual int Id { get; set; }
        public string ModuleName { get; set; }
        public string EntityName { get; set; }
        public string ModelName { get; set; }
        public string SelectFields { get; set; }
    }
}

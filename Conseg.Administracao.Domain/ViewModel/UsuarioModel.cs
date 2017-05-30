using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Conseg.Administracao.Domain.Core;


namespace Conseg.Administracao.Domain.Entities
{    
    public class UsuarioModel : BaseModel
    {
        
        [Required(ErrorMessage ="Campo Obrigatorio")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatorio")]
        [Display(Name ="Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Campo Obrigatorio")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

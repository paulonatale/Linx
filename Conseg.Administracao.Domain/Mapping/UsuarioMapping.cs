using Conseg.Administracao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Mapping
{
    public class UsuarioMapping : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMapping()
        {
            ToTable("Usuario");
            HasKey(d => d.Id);
            Property(d => d.Nome);
            Property(d => d.Login);
            Property(d => d.Password);
        }

    }
}

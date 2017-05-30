using Conseg.Administracao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.DataAccessLayer.dbContext
{
    public class Context : DbContext
    {       
            public Context() : base("IdentityServer") { }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                //modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
                base.OnModelCreating(modelBuilder);
            }

        //   public DbSet<Usuario> User { get; set; }

        //public System.Data.Entity.DbSet<Conseg.Administracao.Domain.ViewModel.UsuarioModel> UsuarioModels { get; set; }
    }
}

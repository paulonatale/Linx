//using Conseg.Administracao.Domain.Core.IRepository;
using Conseg.Administracao.Domain.Core;
using Conseg.Administracao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.DataAccessLayer.dbContext
{
    public class dbContext :  DbContext, IdbContext
    {
        #region Construtor

        //public dbContext() : base("IdentityServer") { }

        public dbContext(string nameOrConnectionString) :base(nameOrConnectionString) { }

        #endregion
        
        #region Utilities



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // todo id e do tipo databasegeneration.identity 
            modelBuilder.Properties<int>()
                .Where(p => p.Name == "Id")
                .Configure(p => p.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));
            // todo rowcersion e do tipo optimistic ocurrency tokem
            modelBuilder.Properties<int>()
               .Where(p => p.Name == "RowVersion")
               .Configure(p => p.IsRowVersion());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            IEnumerable<Assembly> assemblies = GetAssemblies();

            foreach (Assembly ass in assemblies)
            {
                modelBuilder.Configurations.AddFromAssembly(ass);
            }

            base.OnModelCreating(modelBuilder);
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                Set<TEntity>().Attach(entity);
                return entity;
            }
            return alreadyAttached;
        }


        #endregion

        #region Methods

        public string CreateDataBaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoreProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            // adicionando parametros
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not Suport Parameter type");

                    commandText += i == 0 ? " " : ", ";
                    commandText += "@" + p.ParameterName;

                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        // output parameter
                        commandText += " output";
                    }
                }

            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();


            //perfomance hack applied as describe here
            bool acd = this.Configuration.AutoDetectChangesEnabled;


            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;
                for (int i = 0; i < result.Count; i++)
                { result[i] = AttachEntityToContext(result[i]); }
            }
            finally { this.Configuration.AutoDetectChangesEnabled = acd; }

            return result;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            List<Assembly> list, assemblies;
            assemblies = new List<Assembly>();

            list = AppDomain.CurrentDomain.GetAssemblies().Where(d => d.FullName.StartsWith("Conseg")).ToList();
            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(d => d.FullName.StartsWith("Conseg")).ToList());

            foreach (Assembly asm in list)
            {
                foreach (var referenceName in GetReferenceAssembliesByAssembly(asm.GetName()))
                {
                    if (assemblies.Count(x => x.FullName == referenceName.FullName) == 0)
                    {
                        assemblies.Add(AppDomain.CurrentDomain.Load(referenceName));
                    }
                }
            }

            return assemblies;
        }

        public static IEnumerable<AssemblyName> GetReferenceAssembliesByAssembly(AssemblyName asmName)
        {
            Assembly asm = AppDomain.CurrentDomain.Load(asmName);
            List<AssemblyName> listAssemblyName = new List<AssemblyName>();
            foreach (var assemblyName in asm.GetReferencedAssemblies().Where(x => x.FullName.StartsWith("Conseg")))
            {
                listAssemblyName.AddRange(GetReferenceAssembliesByAssembly(assemblyName));
            }
            return listAssemblyName;
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transctionBehavior = doNotEnsureTransaction
                 ? TransactionalBehavior.DoNotEnsureTransaction
                 : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transctionBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            return result;
        }

        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        #endregion

        #region Properties

        public virtual bool ProxyCreationEnabled
        {
            get { return this.Configuration.ProxyCreationEnabled; }
            set { this.Configuration.ProxyCreationEnabled = value; }
        }


        public virtual bool AutoDetectChangesEnabled
        {
            get { return this.Configuration.AutoDetectChangesEnabled; }
            set { this.Configuration.AutoDetectChangesEnabled = value; }
        }


        #endregion
        
    }
}

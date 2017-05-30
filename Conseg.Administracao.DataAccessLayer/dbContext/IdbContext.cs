using Conseg.Administracao.Domain.Core;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conseg.Administracao.DataAccessLayer.dbContext
{
    public interface IdbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IList<TEntity> ExecuteStoreProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new();

        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        void Detach(object entity);

        bool ProxyCreationEnabled { get; set; }

        bool AutoDetectChangesEnabled { get; set; }

    }
}

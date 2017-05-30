using Conseg.Administracao.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public partial interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        OperationResult Insert(T entity);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<OperationResult> InsertAsync(T entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        OperationResult Insert(IEnumerable<T> entities);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<OperationResult> InsertAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        OperationResult Update(T entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<OperationResult> UpdateAsync(T entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        OperationResult Update(IEnumerable<T> entities);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<OperationResult> UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        OperationResult Delete(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<OperationResult> DeleteAsync(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        OperationResult Delete(IEnumerable<T> entities);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<OperationResult> DeleteAsync(IEnumerable<T> entities);

        /// <summary>                
        /// Specifies the related objects to include in the query results.
        /// </summary>
        // Parameters:
        //   source:
        //     The source System.Linq.IQueryable`1 on which to call Include.
        //
        //   path:
        //     The dot-separated list of related objects to return in the query results.
        //
        // Type parameters:
        //   T:
        //     The type of entity being queried.
        //
        // Returns:
        //     A new System.Linq.IQueryable`1 with the defined query path.
        //
        // Remarks:
        //     This extension method calls the Include(String) method of the source System.Linq.IQueryable`1
        //     object, if such a method exists. If the source System.Linq.IQueryable`1 does
        //     not have a matching method, then this method does nothing. The System.Data.Entity.Core.Objects.ObjectQuery`1,
        //     System.Data.Entity.Core.Objects.ObjectSet`1, System.Data.Entity.Infrastructure.DbQuery`1
        //     and System.Data.Entity.DbSet`1 types all have an appropriate Include method to
        //     call. Paths are all-inclusive. For example, if an include call indicates Include("Orders.OrderLines"),
        //     not only will OrderLines be included, but also Orders. When you call the Include
        //     method, the query path is only valid on the returned instance of the System.Linq.IQueryable`1.
        //     Other instances of System.Linq.IQueryable`1 and the object context itself are
        //     not affected. Because the Include method returns the query object, you can call
        //     this method multiple times on an System.Linq.IQueryable`1 to specify multiple
        //     paths for the query.

        IQueryable<T> Include(string path);


        /// Gets a table

        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}

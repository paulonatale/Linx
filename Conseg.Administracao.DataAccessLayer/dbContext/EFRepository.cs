using Conseg.Administracao.Domain.Core;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.DataAccessLayer.dbContext
{
    public partial class EFRrepository<T> : IRepository<T> where T : BaseEntity
    {

        #region Fields

        private readonly IdbContext _context;
        private IDbSet<T> _entities;

        #endregion

        #region Construtor

        public EFRrepository(IdbContext context)
        {
            this._context = context;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return this.Entities.Find(id);
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual Task<T> GetByIdAsync(int id)
        {
            return this.Entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual OperationResult Insert(T entity)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao inserir o registro.";

            try
            {
                if (entity == null)
                {
#if DEBUG
                    message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                this.Entities.Add(entity);

                if (0 == _context.SaveChanges())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    result.AddToEnd(new OperationMessage(message, MessageType.Error));
            //    result.Add(new OperationMessage() { Description = "throw new Exception(GetFullErrorText(dbEx), dbEx)" });
            //}
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<OperationResult> InsertAsync(T entity)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao inserir o registro.";

            try
            {
                if (entity == null)
                {
#if DEBUG
                    message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }
                else
                {
                    this.Entities.Add(entity);
                    result.Entity = entity;

                    if (await _context.SaveChangesAsync() == 0)
                    {
                        result.AddToEnd(new OperationMessage(message, MessageType.Error));
                    }

                    return await Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual OperationResult Insert(IEnumerable<T> entities)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao inserir o(s) registro(s).";

            try
            {
                if (entities == null)
                {
#if DEBUG
                    //message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }
                result.Entity = entities;

                if (_context.SaveChanges() != entities.Count())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<OperationResult> InsertAsync(IEnumerable<T> entities)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao inserir o(s) registro(s).";

            try
            {
                if (entities == null)
                {
#if DEBUG
                    // message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }
                result.Entity = entities;

                if (await _context.SaveChangesAsync() != entities.Count())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual OperationResult Update(T entity)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao atualizar o registro.";

            try
            {
                if (entity == null)
                {
#if DEBUG
                    message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                result.Entity = entity;

                if (0 == _context.SaveChanges())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<OperationResult> UpdateAsync(T entity)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao atualizar o registro.";


            try
            {
                if (entity == null)
                {
#if DEBUG
                    message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                result.Entity = entity;

                if (0 == await _context.SaveChangesAsync())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual OperationResult Update(IEnumerable<T> entities)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao atualizar o(s) registro(s).";

            try
            {
                if (entities == null)
                {
#if DEBUG
                    //message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }
                result.Entity = entities;

                if (_context.SaveChanges() != entities.Count())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }



        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<OperationResult> UpdateAsync(IEnumerable<T> entities)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao atualizar o(s) registro(s).";

            try
            {
                if (entities == null)
                {
#if DEBUG
                    //message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }
                result.Entity = entities;

                if (await _context.SaveChangesAsync() != entities.Count())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual OperationResult Delete(T entity)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao deletar o registro.";

            try
            {
                if (entity == null)
                {
#if DEBUG
                    message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                this.Entities.Remove(entity);

                if (0 == this._context.SaveChanges())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<OperationResult> DeleteAsync(T entity)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao deletar o registro.";

            try
            {
                if (entity == null)
                {
#if DEBUG
                    message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                this.Entities.Remove(entity);

                if (0 == await this._context.SaveChangesAsync())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual OperationResult Delete(IEnumerable<T> entities)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao deletar o(s) registro(s).";

            try
            {
                if (entities == null)
                {
#if DEBUG
                    //message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }
                result.Entity = entities;

                if (_context.SaveChanges() != entities.Count())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }

        public async Task<OperationResult> DeleteAsync(IEnumerable<T> entities)
        {
            OperationResult result = new OperationResult();
            string message = "Erro ao deletar o(s) registro(s).";

            try
            {
                if (entities == null)
                {
#if DEBUG
                    //message = "A entidade [" + entity.GetType().FullName + "] esta nula na classe EFRepository.";
#endif
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));

                    return result;
                }

                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }
                result.Entity = entities;

                if (await _context.SaveChangesAsync() != entities.Count())
                {
                    result.AddToEnd(new OperationMessage(message, MessageType.Error));
                }
            }
            catch (Exception ex)
            {
                result.AddToEnd(new OperationMessage(message, MessageType.Error));
            }

            return result;
        }



        public virtual IQueryable<T> Include(string path)
        {
            return this.Entities.Include(path);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        #endregion

    }
}

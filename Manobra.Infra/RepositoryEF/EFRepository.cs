using Manobra.Domain.Interfaces.Repository;
using Manobra.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Manobra.Infra.RepositoryEF
{
    public class EFRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {

        IConfiguration _configuration;
        protected readonly ManobraContext _dbContext;

        public EFRepository(ManobraContext manobraContext, IConfiguration configuration)
        {
            _dbContext = manobraContext;
            _configuration = configuration;
        }

        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("ManobraConnectionString").Value;
            return connection;
        }

        public TEntity Atualizar(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return entity;
        }

        public IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicado)
        {
            return _dbContext.Set<TEntity>().Where(predicado).AsEnumerable();
        }

        public TEntity BuscarEntidade(Expression<Func<TEntity, bool>> predicado)
        {
            return _dbContext.Set<TEntity>().Where(predicado).FirstOrDefault();
        }

        public TEntity ObterPorId(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> ObterTodos()
        {
            return _dbContext.Set<TEntity>().AsEnumerable();
        }

        public IEnumerable<TEntity> ObterTodosPaginado(int skip, int take)
        {
            return _dbContext.Set<TEntity>().Skip(skip).Take(take).AsEnumerable();
        }

        public void Remover(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public TEntity Salvar(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }
    }
}

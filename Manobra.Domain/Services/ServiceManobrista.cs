using Manobra.Domain.DTO;
using Manobra.Domain.Entities;
using Manobra.Domain.Interfaces.Repository;
using Manobra.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Manobra.Domain.Services
{
    public class ServiceManobrista : IServiceManobrista
    {
        private readonly IRepositoryManobrista _repositoryManobrista;

        public ServiceManobrista(IRepositoryManobrista repositoryManobrista)
        {
            _repositoryManobrista = repositoryManobrista;
        }

        public Manobrista Atualizar(Manobrista entity)
        {
           return _repositoryManobrista.Atualizar(entity);
        }

        public IEnumerable<Manobrista> Buscar(Expression<Func<Manobrista, bool>> predicado)
        {
            return _repositoryManobrista.Buscar(predicado);
        }

        public Manobrista BuscarEntidade(Expression<Func<Manobrista, bool>> predicado)
        {
            return _repositoryManobrista.BuscarEntidade(predicado);
        }

        public Manobrista ObterPorId(int id)
        {
            return _repositoryManobrista.ObterPorId(id);
        }

        public IEnumerable<Manobrista> ObterTodos()
        {
            return _repositoryManobrista.ObterTodos();
        }

        public IEnumerable<Manobrista> ObterTodosPaginado(int skip, int take)
        {
            return _repositoryManobrista.ObterTodosPaginado(skip, take);
        }

        public Manobrista ProcessaAtualizarManobrista(ManobristaDTO manobristaDTO)
        {
            Manobrista manobrista;

            if (manobristaDTO.ManobristaId == 0)
            {
                manobrista = new Manobrista(
                    nome: manobristaDTO.Nome,
                    cpf: manobristaDTO.CPF,
                    nascimento: manobristaDTO.DataNascimento
                    );

                return _repositoryManobrista.Salvar(manobrista);

            }
            else
            {
                manobrista = _repositoryManobrista.ObterPorId(manobristaDTO.ManobristaId);
                manobrista.Nome = manobristaDTO.Nome;
                manobrista.CPF = manobristaDTO.CPF;
                manobrista.DataNascimento = manobristaDTO.DataNascimento;

                return _repositoryManobrista.Atualizar(manobrista);
            }
        }

        public void ProcessaDeletaManobrista(int id)
        {
            var manobrista = _repositoryManobrista.ObterPorId(id);
            _repositoryManobrista.Remover(manobrista);
        }

        public void Remover(Manobrista entity)
        {
            _repositoryManobrista.Remover(entity);
        }

        public Manobrista Salvar(Manobrista entity)
        {
            return _repositoryManobrista.Salvar(entity);
        }
    }
}

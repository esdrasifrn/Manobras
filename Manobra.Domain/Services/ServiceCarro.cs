using Manobra.Domain.DTO;
using Manobra.Domain.Entities;
using Manobra.Domain.Interfaces.Repository;
using Manobra.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Manobra.Domain.Services
{
    public class ServiceCarro : IServiceCarro
    {
        private readonly IRepositoryCarro _repositoryCarro;
        private readonly IRepositoryManobrista _repositoryManobrista;

        public ServiceCarro(IRepositoryCarro repositoryCarro, IRepositoryManobrista repositoryManobrista)
        {
            _repositoryCarro = repositoryCarro;
            _repositoryManobrista = repositoryManobrista;
        }

        public Carro Atualizar(Carro entity)
        {
           return _repositoryCarro.Atualizar(entity);
        }

        public IEnumerable<Carro> Buscar(Expression<Func<Carro, bool>> predicado)
        {
            return _repositoryCarro.Buscar(predicado);
        }

        public Carro BuscarEntidade(Expression<Func<Carro, bool>> predicado)
        {
            return _repositoryCarro.BuscarEntidade(predicado);
        }

        public List<ManobrasDTO> ListaManobrasCarroDTO(string pesquisa)
        {
            var todosCarroscomManobras = _repositoryCarro.ObterTodos();
            var listamanobrasDTOs = new List<ManobrasDTO>();

            foreach (var carro in todosCarroscomManobras)
            {
                foreach (var carroManobra in carro.CarroManobristas.Where(x => x.ResponsavelPelaManobra.Nome.ToUpper().Contains(pesquisa.ToUpper()) ))
                {
                    var carroManobrista = new ManobrasDTO();
                    carroManobrista.NomeCarro = carroManobra.CarroManobrado.Marca;
                    carroManobrista.NomeManobrista = carroManobra.ResponsavelPelaManobra.Nome;
                    carroManobrista.CarroId = carroManobra.CarroId;
                    carroManobrista.ManobristaId = carroManobra.ManobristaId;

                    listamanobrasDTOs.Add(carroManobrista);
                }
            }

            return listamanobrasDTOs;
        }

        public List<ManobrasDTO> ListaManobrasCarroDTOTodos()
        {
            var todosCarroscomManobras = _repositoryCarro.ObterTodos().ToList();
            var listamanobrasDTOs = new List<ManobrasDTO>();

            foreach (var carro in todosCarroscomManobras)
            {
                foreach (var carroManobra in carro.CarroManobristas)
                {
                    var carroManobrista = new ManobrasDTO();
                    carroManobrista.NomeCarro = carroManobra.CarroManobrado.Marca;
                    carroManobrista.NomeManobrista = carroManobra.ResponsavelPelaManobra.Nome;
                    carroManobrista.CarroId = carroManobra.CarroId;
                    carroManobrista.ManobristaId = carroManobra.ManobristaId;

                    listamanobrasDTOs.Add(carroManobrista);
                }
            }

            return listamanobrasDTOs;
        }

        public Carro ObterPorId(int id)
        {
            return _repositoryCarro.ObterPorId(id);
        }

        public IEnumerable<Carro> ObterTodos()
        {
            return _repositoryCarro.ObterTodos();
        }

        public IEnumerable<Carro> ObterTodosPaginado(int skip, int take)
        {
            return _repositoryCarro.ObterTodosPaginado(skip, take);
        }

        public Carro ProcessaAtualizarCarro(CarroDTO carroDTO)
        {
            Carro carro;

            if (carroDTO.CarroId == 0)
            {
                carro = new Carro(
                    marca: carroDTO.Marca,
                    modelo: carroDTO.Modelo,
                    placa: carroDTO.Placa
                    );                

             return  _repositoryCarro.Salvar(carro);

            }
            else
            {
                carro = _repositoryCarro.ObterPorId(carroDTO.CarroId);
                carro.Marca = carroDTO.Marca;
                carro.Modelo = carroDTO.Modelo;
                carro.Placa = carroDTO.Placa;

                return _repositoryCarro.Atualizar(carro);
            }

        }

        public void ProcessaDeletaCarro(int id)
        {
            var carro = _repositoryCarro.ObterPorId(id);
            _repositoryCarro.Remover(carro);
        }

        public Carro ProcessaManobra(ManobrasDTO manobrasDTO)
        {
            Carro carro;
            carro = _repositoryCarro.ObterPorId(manobrasDTO.CarroId);

            Manobrista manobrista;
            manobrista = _repositoryManobrista.ObterPorId(manobrasDTO.ManobristaId);

            CarroManobrista carroManobrista = new CarroManobrista()
            {
                CarroId = manobrasDTO.CarroId,
                ManobristaId = manobrasDTO.ManobristaId,
                CarroManobrado = carro,
                ResponsavelPelaManobra = manobrista
            };

            carro.AddCarroManobrista(carroManobrista);

            return _repositoryCarro.Atualizar(carro);            
        }

        public void Remover(Carro entity)
        {
            _repositoryCarro.Remover(entity);
        }

        public Carro Salvar(Carro entity)
        {
            return _repositoryCarro.Salvar(entity);
        }
    }
}

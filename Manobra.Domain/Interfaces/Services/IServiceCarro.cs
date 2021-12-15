using Manobra.Domain.DTO;
using Manobra.Domain.Entities;
using Manobra.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Domain.Interfaces.Services
{
    public interface IServiceCarro : IServiceBase<Carro>
    {
        public Carro ProcessaAtualizarCarro(CarroDTO carroDTO);
        public void ProcessaDeletaCarro(int id);

        public Carro ProcessaManobra(ManobrasDTO manobrasDTO);

        public List<ManobrasDTO> ListaManobrasCarroDTO(string pesquisa);
        public List<ManobrasDTO> ListaManobrasCarroDTOTodos();
    }
}

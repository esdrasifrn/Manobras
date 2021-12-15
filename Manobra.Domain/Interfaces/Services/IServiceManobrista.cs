using Manobra.Domain.DTO;
using Manobra.Domain.Entities;
using Manobra.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Domain.Interfaces.Services
{
    public interface IServiceManobrista : IServiceBase<Manobrista>
    {
        public Manobrista ProcessaAtualizarManobrista(ManobristaDTO manobristaDTO);
        public void ProcessaDeletaManobrista(int id);
    }
}

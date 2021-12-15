using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manobra.API.Models
{
    public class CarroViewModel
    {
        public int Id { get; set; } = 0;
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
    }
}

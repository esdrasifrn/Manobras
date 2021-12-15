using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Domain.Entities
{
    public class CarroManobrista
    {
        public int ManobristaId { get; set; }
        public virtual Manobrista ResponsavelPelaManobra { get; set; }
        public int CarroId { get; set; }
        public virtual Carro CarroManobrado { get; set; }         
    }
}

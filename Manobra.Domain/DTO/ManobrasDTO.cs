using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Domain.DTO
{
    public class ManobrasDTO
    {
        public string NomeCarro { get; set; }
        public string NomeManobrista { get; set; }
        public int CarroId { get; set; }
        public int ManobristaId { get; set; }
    }
}

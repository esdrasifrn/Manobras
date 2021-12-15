using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Domain.DTO
{
    public class ManobristaDTO
    {
        public int ManobristaId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}

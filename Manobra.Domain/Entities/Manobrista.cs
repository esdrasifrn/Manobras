using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Domain.Entities
{
    public class Manobrista
    {
        public Manobrista(string nome, string cpf, DateTime nascimento)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = nascimento;
        }

        public Manobrista()
        {

        }

        public virtual ICollection<CarroManobrista> CarroManobristas { get; set; }
        public int ManobristaId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}

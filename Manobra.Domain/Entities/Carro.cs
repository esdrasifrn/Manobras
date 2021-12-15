using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Domain.Entities
{
    public class Carro
    {
        public Carro(string marca, string modelo, string placa)
        {
            Marca = marca;
            Modelo = modelo;
            Placa = placa;
            _carroManobristas = new List<CarroManobrista>();
        }

        public Carro()
        {
            _carroManobristas = new List<CarroManobrista>();
        }

        private IList<CarroManobrista> _carroManobristas;
        public int CarroId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public virtual ICollection<CarroManobrista> CarroManobristas { get => _carroManobristas; private set { } }


        public void AddCarroManobrista(CarroManobrista carroManobrista)
        {
            _carroManobristas.Add(carroManobrista);
        }

    }
}

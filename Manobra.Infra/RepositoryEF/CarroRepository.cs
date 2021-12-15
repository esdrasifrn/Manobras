using Manobra.Domain.Entities;
using Manobra.Domain.Interfaces.Repository;
using Manobra.Infra.Data;
using Manobra.Infra.RepositoryEF;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Infra.RepositoryEF
{
    public class CarroRepository : EFRepository<Carro>, IRepositoryCarro
    {
        public CarroRepository(ManobraContext manobraContext, IConfiguration configuration) : base(manobraContext, configuration)
        {

        }
    }
}

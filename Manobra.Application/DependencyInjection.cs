using Manobra.Domain.Interfaces.Repository;
using Manobra.Domain.Interfaces.Services;
using Manobra.Domain.Services;
using Manobra.Infra.Http;
using Manobra.Infra.Interfaces;
using Desafio.Infra.RepositoryEF;
using Microsoft.AspNetCore.Http;
using SDK.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Manobra.Application
{
    public class DependencyInjection
    {
        public static void RegisterDependencies(ISdkContainerBuilder builder)
        {            
            RegisterManobraDependencies(builder);
        }

        private static void RegisterManobraDependencies(ISdkContainerBuilder builder)
        {
            builder.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.RegisterScoped<IHttpUserAgent>(c =>
             new ClientAgent("http://host.docker.internal:20001/",
             new HttpClientHandler
             {
                 ServerCertificateCustomValidationCallback =
             (message, cert, chain, errors) => { return true; }
             })
            );

            builder.RegisterScoped<IRepositoryCarro, CarroRepository>();
            builder.RegisterScoped<IRepositoryManobrista, ManobristaRepository>();
            builder.RegisterScoped<IServiceCarro, ServiceCarro>();
            builder.RegisterScoped<IServiceManobrista, ServiceManobrista>();
        }
    }
}
